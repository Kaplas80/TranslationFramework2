using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PHM1")]
    [Serializable]
    public sealed class PHM1 : Condition
    {
        private PHM1(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = info.GetBoolean("PHM1:I");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PHM1:I", _i);
        }

        private bool _i;
    }
}