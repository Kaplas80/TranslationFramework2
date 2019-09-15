using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("C01")]
    [Serializable]
    public sealed class C01 : DE2
    {
        private C01(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (ItemInstance)info.GetValue("C01:I", typeof(ItemInstance));
            _a = info.GetDouble("C01:A");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("C01:I", _i);
            info.AddValue("C01:A", _a);
        }

        private ItemInstance _i;

        private double _a;
    }
}
