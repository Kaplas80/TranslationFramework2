using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Aspects;

namespace UnderRailLib.Models
{
    [EncodedTypeName("RGEA")]
    [Serializable]
    public sealed class RGEA : Aspect<LE2>, iINA, iLBL
    {
        private RGEA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _c = (info.GetValue("RGEA:C", typeof(List<string>)) as List<string>);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("RGEA:C", _c);
        }

        public List<string> _c = new List<string>();
    }
}
