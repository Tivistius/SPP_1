using YamlDotNet.Serialization;
using System.IO;
using Tracer.Serialization.Abstractions;
using Tracer.Core;

public class YamlTraceResultSerializer : ITraceResultSerializer
{
    public string Format => "yaml";

    public void Serialize(IReadOnlyTraceResult traceResult, Stream to)
    {
        var serializer = new Serializer();
        using (var writer = new StreamWriter(to))
        {
            serializer.Serialize(writer, traceResult);
        }
    }
}
