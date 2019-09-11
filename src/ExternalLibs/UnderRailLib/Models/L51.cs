using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("L51")]
    [Serializable]
    public sealed class L51 : A50, ISerializable
    {
        private L51(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _p = (info.GetValue("L51:P", typeof(bool?)) as bool?);
            _o = (info.GetValue("L51:O", typeof(bool?)) as bool?);
            _l = info.GetString("L51:L");
            _b = info.GetString("L51:B");
            if (DataModelVersion.MinorVersion >= 129)
            {
                _t = (info.GetValue("L51:T", typeof(eO0?)) as eO0?);
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("L51:P", _p);
            info.AddValue("L51:O", _o);
            info.AddValue("L51:L", _l);
            info.AddValue("L51:B", _b);
            info.AddValue("L51:T", _t);
        }

        private bool? _p;

        private bool? _o;

        private string _l;

        private string _b;

        private eO0? _t;
    }
}
