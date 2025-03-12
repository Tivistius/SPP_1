using System.Xml.Serialization;
using Tracer.Serialization.Abstractions;
using Tracer.Core;
using System.IO;

public class XmlTraceResultSerializer : ITraceResultSerializer
{
    public string Format => "xml";

    public void Serialize(IReadOnlyTraceResult traceResult, Stream to)
    {
        var xmlSerializer = new XmlSerializer(typeof(TraceResult));
        xmlSerializer.Serialize(to, traceResult);
    }
}
