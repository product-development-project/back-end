using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pvp.Data.Dto;
using pvp.Data.Entities;
using pvp.Data.Repositories;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace pvp.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/code/checker")]
    public class CodeCheckerController : ControllerBase
    {
        private readonly IResultRepository _resultRepository;
        private readonly ILoggedRepository _loggedRepository;
        private readonly ISolutionRepository _solutionRepositry;

        public CodeCheckerController(IResultRepository resultRepository, ISolutionRepository solutionRepository, ILoggedRepository loggedRepository)
        {
            _resultRepository = resultRepository;
            _loggedRepository = loggedRepository;
            _solutionRepositry = solutionRepository;
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
            int taskId = requestBody.taskId;
            int? adId = requestBody.adId;
            string userId = requestBody.userId;

            var testCases = new List<Tuple<string, string>>();

            if (type == "exercise" || type == "competition")
            {
                string exerciseName = requestBody.name;

                var results = await _resultRepository.GetManyAsyncByTask(taskId);
                testCases = results.Select(o => Tuple.Create(o.Duomenys, o.Rezultatas)).ToList();
            }

            int amountOfTests = testCases.Count;

            List<string> passedList = new List<string> { };
            List<string> failedList = new List<string> { };
            double runTime = 0;
            double memoryUsage = 0;
            int testCaseNumber = 0;

            foreach (var testCase in testCases)
            {
                testCaseNumber++;
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
                                    passedList.Add($"Test case {testCaseNumber} passed.");
                                }
                                else
                                {
                                    failedList.Add($"Test case {testCaseNumber} failed.");
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

            if (type == "exercise")
            {
                var loggedUser = await _loggedRepository.GetAsyncByUserIdTaskId(userId, taskId);

                if (loggedUser == null) {
                    var logged = new Prisijunge
                    {
                        UserId = userId,
                        Skelbimas_id = null,
                        Uzduotys_id = taskId
                    };

                    await _loggedRepository.CreateAsync(logged);
                    loggedUser = await _loggedRepository.GetAsyncByUserIdTaskId(userId, taskId);
                }

                var solution = await _solutionRepositry.GetAsyncByUserId(loggedUser.Id);
                var codeInBytes = Encoding.UTF8.GetBytes(code);

                if (solution == null)
                {
                    var result = new Sprendimas
                    {
                        Programa = codeInBytes,
                        ProgramosLaikas = runTime,
                        RamIsnaudojimas = memoryUsage,
                        Prisijunge_id = loggedUser.Id,
                        ParinktosUzduotys_id = null,
                        Teisingumas = getTaskPoints(passedArray, amountOfTests),
                        ResursaiTaskai = getRamUsagePoints(memoryUsage),
                        ProgramosLaikasTaskai = getRunTimePoints(runTime)
                    };

                    await _solutionRepositry.CreateAsync(result);
                }
                else
                {
                    solution.Programa = codeInBytes;
                    solution.ProgramosLaikas = runTime;
                    solution.RamIsnaudojimas = memoryUsage;
                    solution.Teisingumas = getTaskPoints(passedArray, amountOfTests);
                    solution.ResursaiTaskai = getRamUsagePoints(memoryUsage);
                    solution.ProgramosLaikasTaskai = getRunTimePoints(runTime);

                    await _solutionRepositry.UpdateAsync(solution);
                }
            }
            else if (type == "competition")
            {
                var loggedUser = await _loggedRepository.GetAsyncByUserIdAdIdTaskId(userId, adId, taskId);

                if (loggedUser == null && adId != null && taskId != null)
                {
                    var logged = new Prisijunge
                    {
                        UserId = userId,
                        Skelbimas_id = adId,
                        Uzduotys_id = taskId
                    };

                    await _loggedRepository.CreateAsync(logged);
                    loggedUser = await _loggedRepository.GetAsyncByUserIdAdIdTaskId(userId, adId, taskId);
                }

                var solution = await _solutionRepositry.GetAsyncByUserId(loggedUser.Id);
                var codeInBytes = Encoding.UTF8.GetBytes(code);

                if (solution == null)
                {
                    var result = new Sprendimas
                    {
                        Programa = codeInBytes,
                        ProgramosLaikas = runTime,
                        RamIsnaudojimas = memoryUsage,
                        Prisijunge_id = loggedUser.Id,
                        ParinktosUzduotys_id = null,
                        Teisingumas = getTaskPoints(passedArray, amountOfTests),
                        ResursaiTaskai = getRamUsagePoints(memoryUsage),
                        ProgramosLaikasTaskai = getRunTimePoints(runTime)
                    };

                    await _solutionRepositry.CreateAsync(result);
                }
                else
                {
                    solution.Programa = codeInBytes;
                    solution.ProgramosLaikas = runTime;
                    solution.RamIsnaudojimas = memoryUsage;
                    solution.Teisingumas = getTaskPoints(passedArray, amountOfTests);
                    solution.ResursaiTaskai = getRamUsagePoints(memoryUsage);
                    solution.ProgramosLaikasTaskai = getRunTimePoints(runTime);
                    await _solutionRepositry.UpdateAsync(solution);
                }
            }

            return new CodeResultDto(passedArray, failedArray, runTime, memoryUsage, getTaskPoints(passedArray, amountOfTests), getRamUsagePoints(memoryUsage), getRunTimePoints(runTime));
        }

        private int getTaskPoints(string[] passed, int amountOfTests)
        {
            int score = 0;
            if (passed.Length == 0)
            {
                return score;
            }
            else if (passed.Length == amountOfTests)
            {
                score += 100;
            }
            else
            {
                int pointsTestCase = 100 / amountOfTests;
                score += 100 - (pointsTestCase * (amountOfTests - passed.Length));
            }

            return score;
        }

        private int getRamUsagePoints(double memoryUsage)
        {
            int score = 0;

            if (memoryUsage == 0)
            {
                return score;
            }
            else if (memoryUsage < 7.0)
            {
                score += 15;
            }
            else if (memoryUsage < 7.7)
            {
                score += 10;
            }
            else if (memoryUsage < 8.0)
            {
                score += 5;
            }

            return score;
        }

        private int getRunTimePoints(double runTime)
        {
            int score = 0;
            if (runTime == 0)
            {
                return score;
            }
            else if (runTime < 0.01)
            {
                score += 20;
            }
            else if (runTime < 0.05)
            {
                score += 10;
            }

            return score;
        }
    }
}
