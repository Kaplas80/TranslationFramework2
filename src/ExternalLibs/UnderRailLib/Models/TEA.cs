using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Aspects;

namespace UnderRailLib.Models
{
    [EncodedTypeName("TEA")]
    [Serializable]
    public sealed class TEA : Aspect<LE2>, d2a
    {
        private TEA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion >= 27)
            {
                _upt = (info.GetValue("TEA:UPT", typeof(List<string>)) as List<string>);
                if (DataModelVersion.MinorVersion >= 271)
                {
                    _d = info.GetBoolean("TEA:D");
                }
            }
            else
            {
                _upt = new List<string>();
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("TEA:UPT", _upt);
            info.AddValue("TEA:D", _d);
        }

        public List<string> _upt = new List<string>();

        public bool _d;
    }
}
