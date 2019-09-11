using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("L50")]
    [Serializable]
    public sealed class L50 : A50, ISerializable
    {
        private L50(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _p = (info.GetValue("L50:P", typeof(bool?)) as bool?);
            _o = (info.GetValue("L50:O", typeof(bool?)) as bool?);
            _l = info.GetString("L50:L");
            if (DataModelVersion.MinorVersion >= 129)
            {
                _t = (info.GetValue("L50:T", typeof(eL0?)) as eL0?);
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("L50:P", _p);
            info.AddValue("L50:O", _o);
            info.AddValue("L50:L", _l);
            info.AddValue("L50:T", _t);
        }

        private bool? _p;

        private bool? _o;

        private string _l;

        private eL0? _t;
    }
}
