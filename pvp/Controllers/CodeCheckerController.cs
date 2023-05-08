﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pvp.Data.Dto;
using pvp.Data.Repositories;
using System.Net;
using System.Text;

namespace pvp.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/code/checker")]
    public class CodeCheckerController : ControllerBase
    {
        private readonly IResultRepository _resultRepository;

        public CodeCheckerController(IResultRepository resultRepository)
        {
            _resultRepository = resultRepository;
        }

        [HttpPost]
        //[Authorize(Roles = UserRoles.Alll)]
        public async Task<ActionResult<CodeResultDto>> RunCode(CodeChekcerDto requestBody)
        {
            string endpoint = "https://api.jdoodle.com/v1/execute";
            //reikės paskui kažkur saugiai padėti šitus pvz Azure key vault
            string clientId = "5c30f90c511a11b6effd3efe10e13103";
            string clientSecret = "d6a5fa726f7cb80f5744d6c3d45f077ad53e6192b6a97d690037a56905aceb91";

            string code = requestBody.code;
            string language = requestBody.language;
            string type = requestBody.type;
            string versionIndex = "4";
            int id = requestBody.taskId;
            var testCases = new List<Tuple<string, string>>();

            if (type == "exercise")
            {
                string exerciseName = requestBody.name;

                var results = await _resultRepository.GetManyAsyncByTask(id);
                testCases = results.Select(o => Tuple.Create(o.Duomenys, o.Rezultatas)).ToList();
            }

            List<string> passedList = new List<string> { };
            List<string> failedList = new List<string> { };
            double runTime = 0;
            double memoryUsage = 0;

            foreach (var testCase in testCases)
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
                                    passedList.Add($"Test case {testCase.Item1} passed. Expected: {testCase.Item2}. Actual: {responseJson["output"].ToString().Trim()}");
                                }
                                else
                                {
                                    failedList.Add($"Test case {testCase.Item1} failed. Expected: {testCase.Item2}. Actual: {responseJson["output"].ToString().Trim()}");
                                }
                                if (double.TryParse(responseJson["cpuTime"]?.ToString().Trim(), out var runTimeDecimal))
                                {
                                    if (runTimeDecimal > runTime)
                                    {
                                        runTime = runTimeDecimal;
                                    }
                                }
                                if (double.TryParse(responseJson["memory"]?.ToString().Trim(), out var memoryDecimal))
                                {
                                    memoryDecimal /= 1024;
                                    if (memoryDecimal > memoryUsage)
                                    {
                                        memoryUsage = memoryDecimal;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            string[] passedArray = passedList.ToArray();
            string[] failedArray = failedList.ToArray();

            return new CodeResultDto(passedArray, failedArray, runTime, memoryUsage);
        }
    }
}
