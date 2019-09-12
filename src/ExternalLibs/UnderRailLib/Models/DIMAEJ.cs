using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DIMAEJ")]
    [Serializable]
    public sealed class DIMAEJ : Job
    {
        private DIMAEJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _l = info.GetString("DIMAEJ:L");
            _c = info.GetString("DIMAEJ:C");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("DIMAEJ:L", _l);
            info.AddValue("DIMAEJ:C", _c);
        }

        private string _l;

        private string _c;
    }
}
