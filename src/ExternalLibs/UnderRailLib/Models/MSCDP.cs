using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.TimelapseVertigo.Rules.Capabilities;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MSCDP")]
    [Serializable]
    public sealed class MSCDP : MSPE2, iMS
    {
        private MSCDP(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _c = (SpecialCategory)info.GetValue("MSCDP:C", typeof(SpecialCategory));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MSCDP:C", _c);
        }

        private SpecialCategory _c;
    }
}
