using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MIFV")]
    [Serializable]
    public sealed class MIFV : SetFieldValueAction
    {
        private MIFV(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _value = info.GetInt32("MIFV:V");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MIFV:V", _value);
        }

        private int _value;
    }
}
