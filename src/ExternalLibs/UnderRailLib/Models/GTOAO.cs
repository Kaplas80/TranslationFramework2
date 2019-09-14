using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("GTOAO")]
    [Serializable]
    public sealed class GTOAO : Job
    {
        private GTOAO(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _e = info.GetBoolean("GTOAO:E");
            _ogc = info.GetString("GTOAO:OGC");
            SerializationHelper.ReadList("GTOAO:GS", ref _gs, info);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("GTOAO:E", _e);
            info.AddValue("GTOAO:OGC", _ogc);
            SerializationHelper.WriteList("GTOAO:GS", _gs, info);
        }

        private bool _e;

        private string _ogc;

        private List<string> _gs = new List<string>();
    }
}
