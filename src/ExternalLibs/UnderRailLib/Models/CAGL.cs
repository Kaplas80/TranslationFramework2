using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CAGL")]
    [Serializable]
    public sealed class CAGL : Condition
    {
        private CAGL(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?) info.GetValue("CAGL:I", typeof(Guid?));
            _a = info.GetString("CAGL:A");
            _n = info.GetBoolean("CAGL:N");
            _s = info.GetString("CAGL:S");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CAGL:I", _i);
            info.AddValue("CAGL:A", _a);
            info.AddValue("CAGL:N", _n);
            info.AddValue("CAGL:S", _s);
        }

        private Guid? _i;

        private string _a;

        private bool _n;

        private string _s;
    }
}
