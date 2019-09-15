using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("GBIX")]
    [Serializable]
    public sealed class GBIX : NonEquippableItem
    {
        private GBIX(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            a = (info.GetValue("GBIX:E", typeof(IFX)) as IFX);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("GBIX:E", a);
        }

        private new IFX a;
    }
}
