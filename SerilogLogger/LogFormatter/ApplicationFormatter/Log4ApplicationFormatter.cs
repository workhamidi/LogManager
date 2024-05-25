using System.Text;
using Serilog.Events;
using Serilog.Formatting;

namespace SerilogLogger.LogFormatter.ApplicationFormatter;
public class Log4ApplicationFormatter : ITextFormatter
{
    private static int _counter = 0;
    
    public void Format(LogEvent logEvent, TextWriter output)
    {
        var stringBuilder = new StringBuilder();
        
        stringBuilder.Append("<event>");
        stringBuilder.Append(logEvent.RenderMessage());
        stringBuilder.Append("</event>");

        output.Write(stringBuilder.ToString());

        stringBuilder.Clear();

        output.Flush();

        output.Close();

        _counter++;

        if (_counter % 1000 == 0)
        {
            GC.Collect();

            _counter = 0;
        }
    }
}