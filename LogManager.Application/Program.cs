using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SerilogLogger;
using SerilogLogger.Dto;
using SerilogLogger.Interface;
using SerilogLogger.Utilities;


namespace LogManager.Application;

public class Program
{
    private readonly string _className;

    public Program()
    {
        _className = this.GetType().Name;
    }



    public static void Main()
    {

        new Program().TestNormalLog();

        new Program().TestQueueLog();
    }



    private void TestNormalLog()
    {

        var builder = Host.CreateDefaultBuilder()
            .UseSerilog()
            .ConfigureServices((context, services) =>
            {
                _ = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json",
                        optional: true, reloadOnChange: true)
                    .Build();


                var applicationConfiguration = context.Configuration
                    .GetSection(nameof(ApplicationLogConfiguration))
                    .Get<ApplicationLogConfiguration>()!;


                services.AddLoggerDependencies(applicationConfiguration);

            });
        
        var host = builder.Build();

        var log = host.Services.GetService<ILog>()!;

        new Program().Log(log);

        host.Run();

    }

    private void TestQueueLog()
    {

        var builder = Host.CreateDefaultBuilder()
            .UseSerilog()
            .ConfigureServices((context, services) =>
            {
                _ = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json",
                        optional: true, reloadOnChange: true)
                    .Build();


                var applicationConfiguration = context.Configuration
                    .GetSection(nameof(ApplicationLogConfiguration))
                    .Get<ApplicationLogConfiguration>()!;


                applicationConfiguration.IsLogToQueue = true;

                services.AddLoggerDependencies(applicationConfiguration);

            });

        var host = builder.Build();

        var log = host.Services.GetService<ILog>()!;

        new Program().Log(log);

        host.Run();

    }


    public void Log(ILog log)
    {
        var methodName = MethodName.GetName();


        /************ normal log ************/


        log.Information("test log",
            new List<KeyValuePair<string, object?>>()
            {
                new("class name", _className),
                new("method name", methodName)
            }!);

        /************ add exception to log ************/

        var exception = new Exception("test exception");

        log.Error("test log",
            new List<KeyValuePair<string, object?>>()
            {
                new("class name", _className),
                new("method name", methodName)
            }!, exception);



        /************ use stack trace ************/


        log.Error("test log",
            new List<KeyValuePair<string, object?>>()
            {
                new("class name", _className),
                new("method name", methodName)
            }!, exception, true);
    }
}