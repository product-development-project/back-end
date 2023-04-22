using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pvp.Auth.Models;
using pvp.Data.Entities;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;

namespace pvp.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/code/checker")]
    public class CodeCheckerController : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = UserRoles.Alll)]
        public async Task<IActionResult> RunCode([FromBody] JObject requestBody)
        {
            if (requestBody == null)
            {
                return BadRequest();
            }

            string endpoint = "https://api.jdoodle.com/v1/execute";
            //reikės paskui kažkur saugiai padėti šitus pvz Azure key vault
            string clientId = "5c30f90c511a11b6effd3efe10e13103";
            string clientSecret = "d6a5fa726f7cb80f5744d6c3d45f077ad53e6192b6a97d690037a56905aceb91";

            string code = (string)requestBody["code"];
            string language = (string)requestBody["language"];
            string type = (string)requestBody["type"];
            string versionIndex;
            string[] languagesToMatch = { "c", "cpp" };
            var testCases = new List<Tuple<string, string>>();

            JsonObject responseData = new JsonObject();

            if (languagesToMatch.Any(lang => language.Contains(lang)))
            {
                versionIndex = "5";
            }
            else
            {
                versionIndex = "4";
            }

            if (type == "exercise")
            {
                string exerciseName = (string)requestBody["name"];

                var testCasesJson = System.IO.File.ReadAllText("test-cases.json");
                var cases = JsonConvert.DeserializeObject<List<TestCase>>(testCasesJson);

                testCases = cases.Where(c => c.exercise == exerciseName)
                 .Select(c => Tuple.Create(c.input, c.expectedOutput))
                 .ToList();
            }

            foreach (var testCase in testCases)
            {
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(endpoint);
                    request.Method = "POST";
                    request.ContentType = "application/json";

                    string postData = JsonConvert.SerializeObject(new
                    {
                        clientId = clientId,
                        clientSecret = clientSecret,
                        script = code,
                        language = language,
                        versionIndex = versionIndex,
                        stdin = testCase.Item1
                    });

                    byte[] data = Encoding.UTF8.GetBytes(postData);

                    request.ContentLength = data.Length;

                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }

                    using (var response = await request.GetResponseAsync())
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                var responseString = await reader.ReadToEndAsync();
                                var responseJson = JObject.Parse(responseString);

                                if (responseJson["statusCode"].ToString() == "200")
                                {
                                    if (responseJson["output"].ToString().Trim() == testCase.Item2.Trim())
                                    {
                                        responseData.Add("passed", $"Test case {testCase.Item1} passed.");
                                    }
                                    else
                                    {
                                        responseData.Add("failed", $"Test case {testCase.Item1} failed. Expected: {testCase.Item2}. Actual: {responseJson["output"].ToString().Trim()}");
                                    }
                                }
                                else
                                {
                                    return BadRequest(responseJson);
                                }
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine(ex.ToString());
                    var errorResponse = ex.Response as HttpWebResponse;
                    var errorMessage = errorResponse != null ? new StreamReader(errorResponse.GetResponseStream()).ReadToEnd() : ex.Message;
                    return BadRequest($"An error occurred while executing the code: {errorMessage}");
                }
            }

            return Ok(responseData);
        }
    }
}
