﻿using SerilogLogger.Enum;

namespace SerilogLogger.Dto;


public class LogQueueDto
{
    public LogQueueDto(
        ELogLevel logLevel,
        string logMessage,
        List<KeyValuePair<string, object>>? logParameters = null,
        Exception? logException = null,
        bool isStackTraceEnable=false
    )
    {
        LogLevel = logLevel;
        LogMessage = logMessage;
        LogParameters = logParameters;
        LogException = logException;
        IsStackTraceEnable = isStackTraceEnable;
        LogTime = DateTime.Now;
    }

    private LogQueueDto()
    {

    }

    public ELogLevel LogLevel { get; set; }

    public string LogMessage { get; set; } = null!;

    public List<KeyValuePair<string, object>>? LogParameters { get; set; }

    public Exception? LogException { get; set; }

    public bool IsStackTraceEnable { get; set; }

    public DateTime LogTime { get; private set; } = DateTime.Now;

}
