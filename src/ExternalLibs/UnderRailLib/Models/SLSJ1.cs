using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SLSJ1")]
    [Serializable]
    public sealed class SLSJ1 : Job
    {
        private SLSJ1(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?) info.GetValue("SLSJ1:I", typeof(Guid?));
            _a = info.GetString("SLSJ1:A");
            _l = info.GetBoolean("SLSJ1:L");
            _j = info.GetBoolean("SLSJ1:J");
            _s = info.GetBoolean("SLSJ1:S");
            if (DataModelVersion.MinorVersion >= 234)
            {
                _p = info.GetBoolean("SLSJ1:P");
                return;
            }

            _p = true;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SLSJ1:I", _i);
            info.AddValue("SLSJ1:A", _a);
            info.AddValue("SLSJ1:L", _l);
            info.AddValue("SLSJ1:J", _j);
            info.AddValue("SLSJ1:S", _s);
            info.AddValue("SLSJ1:P", _p);
        }

        private bool _l;

        private Guid? _i;

        private string _a;

        private bool _j;

        private bool _s;

        private bool _p;
    }
}
