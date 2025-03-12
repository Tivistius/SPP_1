﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracer.Core;

namespace Tracer.Serialization.Abstractions
{
    public interface ITraceResultSerializer
    {
        string Format { get; }
        void Serialize(IReadOnlyTraceResult traceResult, Stream to);

    }

}
