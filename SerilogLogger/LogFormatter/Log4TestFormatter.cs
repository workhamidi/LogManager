using System.Runtime.Serialization;

namespace SerilogLogger.LogFormatter
{
    public class Log4TestFormatter : IFormatter
    {
        public object Deserialize(Stream serializationStream)
        {
            throw new NotImplementedException();
        }

        public void Serialize(Stream serializationStream, object graph)
        {
            throw new NotImplementedException();
        }

        public SerializationBinder? Binder { get; set; }

        public StreamingContext Context { get; set; }

        public ISurrogateSelector? SurrogateSelector { get; set; }
    }
}
