using System.Collections.Concurrent;
using Serilog.Events;
using SerilogLogger.Dtos;
using SerilogLogger.LoggerInterface;


namespace SerilogLogger.LoggerImplementation.LogToQueue;

public class SeriLogQueueLogger : BaseSeriLog, ILog
{
    private BlockingCollection<LogDto> _logs = new();

    public SeriLogQueueLogger(string applicationId, string applicationName) : base(applicationId, applicationName)
        => Task.Run(ProcessorLogs);

    private void ProcessorLogs()
    {

        var counter = 0;

        var timer = new System.Timers.Timer(2_000);

        timer.Elapsed += (_, _) =>
        {
            if (_logs.Count == 0)
            {
                if (counter < 10)
                {
                    GC.Collect();
                }
                else timer.Stop();

                counter++;
            }

        };

        timer.Start();


        foreach (var log in _logs.GetConsumingEnumerable())
        {
            if (timer.Enabled is false)
            {
                counter = 0;

                timer.Start();
            }

            using (log)
                SendLog(log.LogLevel, log.LogMessage, log.LogException, log.LogParameters);

            if (_logs.Count % 5_000 == 0) GC.Collect();
        }
    }

    public void Debug(string messageTemplate, List<KeyValuePair<string, object>>? parameters = null, Exception? exception = null)
        => _logs.Add(new LogDto(LogEventLevel.Debug, messageTemplate, parameters, exception));

    public void Error(string messageTemplate, List<KeyValuePair<string, object>>? parameters = null, Exception? exception = null)
        => _logs.Add(new LogDto(LogEventLevel.Error, messageTemplate, parameters, exception));

    public void Fatal(string messageTemplate, List<KeyValuePair<string, object>>? parameters = null, Exception? exception = null)
        => _logs.Add(new LogDto(LogEventLevel.Fatal, messageTemplate, parameters, exception));

    public void Information(string messageTemplate, List<KeyValuePair<string, object>>? parameters = null, Exception? exception = null)
        => _logs.Add(new LogDto(LogEventLevel.Information, messageTemplate, parameters, exception));

    public void Verbose(string messageTemplate, List<KeyValuePair<string, object>>? parameters = null, Exception? exception = null)
        => _logs.Add(new LogDto(LogEventLevel.Verbose, messageTemplate, parameters, exception));

    public void Warning(string messageTemplate, List<KeyValuePair<string, object>>? parameters = null, Exception? exception = null)
        => _logs.Add(new LogDto(LogEventLevel.Warning, messageTemplate, parameters, exception));
}
