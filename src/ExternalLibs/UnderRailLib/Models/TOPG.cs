using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("TOPG")]
    [Serializable]
    public sealed class TOPG : Job
    {
        private TOPG(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion >= 230)
            {
                _p = (info.GetValue("TOPG:P", typeof(Guid?)) as Guid?);
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            if (DataModelVersion.MinorVersion >= 230)
            {
                info.AddValue("TOPG:P", _p);
            }
        }

        private Guid? _p;
    }
}
