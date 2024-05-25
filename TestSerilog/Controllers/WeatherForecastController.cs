using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SerilogLogger.LoggerInterface;

namespace TestSerilog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILog _logger;

        public WeatherForecastController(ILog logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "GetLogTime")]
        public string GetLogTime(int count)
        {

            var stopWatch = new Stopwatch();

            stopWatch.Start();


            Enumerable.Range(1, count).AsParallel()
                .WithDegreeOfParallelism(20)
                .ForAll((i) =>
                {
                    _logger.Information($"{i}-Test logger speed."

                        , new List<KeyValuePair<string, object>>
                        {
                            new KeyValuePair<string, object>(Guid.NewGuid().ToString().Substring(0, 5) + "- 1", Guid.NewGuid().ToString().Substring(0, 5)),
                            new KeyValuePair<string, object>(Guid.NewGuid().ToString().Substring(0, 5) + "- 2", Guid.NewGuid().ToString().Substring(0, 5)),
                            new KeyValuePair<string, object>(Guid.NewGuid().ToString().Substring(0, 5) + "- 3", Guid.NewGuid().ToString().Substring(0, 5)),
                            new KeyValuePair<string, object>(Guid.NewGuid().ToString().Substring(0, 5) + "- 4", Guid.NewGuid().ToString().Substring(0, 5)),
                            new KeyValuePair<string, object>(Guid.NewGuid().ToString().Substring(0, 5) + "- 5", Guid.NewGuid().ToString().Substring(0, 5))
                        }
                            );
                });

            stopWatch.Stop();

            var time = stopWatch.Elapsed.TotalMilliseconds;

            return time.ToString();
        }
    }
}