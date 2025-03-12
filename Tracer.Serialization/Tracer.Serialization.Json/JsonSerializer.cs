using Newtonsoft.Json;
using System.IO;
using Tracer.Core;
using Tracer.Serialization.Abstractions;

public class JsonTraceResultSerializer : ITraceResultSerializer
{
    public string Format => "json";

    public void Serialize(IReadOnlyTraceResult traceResult, Stream to)
    {
        using (var writer = new StreamWriter(to))
        {
            var json = JsonConvert.SerializeObject(traceResult, Formatting.Indented);
            writer.Write(json);
        }
    }
}