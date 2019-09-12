using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("IRG")]
    [Serializable]
    public sealed class ItemRangeGenerator : ItemGeneratorBase
    {
        private ItemRangeGenerator(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _ir = (eIR) info.GetValue("IRG:IR", typeof(eIR));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("IRG:IR", _ir);
        }

        private eIR _ir;
    }
}
