using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("R51")]
    [Serializable]
    public sealed class R51 : A50
    {
        private R51(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _p = (info.GetValue("R51:P", typeof(bool?)) as bool?);
            _o = (info.GetValue("R51:O", typeof(bool?)) as bool?);
            _l = info.GetString("R51:L");
            _b = info.GetString("R51:B");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("R51:P", _p);
            info.AddValue("R51:O", _o);
            info.AddValue("R51:L", _l);
            info.AddValue("R51:B", _b);
        }

        private bool? _p;

        private bool? _o;

        private string _l;

        private string _b;
    }
}
