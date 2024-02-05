using Microsoft.Extensions.DependencyInjection;
using SerilogLogger.Concrete.LogToQueue;
using SerilogLogger.Concrete.NormalLog;
using SerilogLogger.Dto;
using SerilogLogger.Interface;

namespace SerilogLogger;

public static class DependencyInjection
{
    public static IServiceCollection AddLoggerDependencies(this IServiceCollection services, ApplicationLogConfiguration logConfiguration)
    {
        if (logConfiguration.IsLogToQueue)
            services.AddSingleton<ILog, SerilogQueueLogger>(_ =>
                new SerilogQueueLogger(logConfiguration.ApplicationId, logConfiguration.ApplicationName));
        
        else
            services.AddSingleton<ILog, SerilogNormalLogger>(_ =>
                    new SerilogNormalLogger(logConfiguration.ApplicationId, logConfiguration.ApplicationName));
        
        return services;
    }
}