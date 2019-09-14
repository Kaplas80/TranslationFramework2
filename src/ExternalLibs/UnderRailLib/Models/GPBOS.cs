using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("GPBOS")]
    [Serializable]
    public sealed class GPBOS : Job
    {
        private GPBOS(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _gp = info.GetString("GPBOS:GP");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("GPBOS:GP", _gp);
        }

        private string _gp;
    }
}
