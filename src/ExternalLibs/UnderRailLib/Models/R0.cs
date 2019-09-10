using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("R0")]
    [Serializable]
    public sealed class R0 : Job, ISerializable
    {
        private R0(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _s = (info.GetValue("R0:S", typeof(Guid?)) as Guid?);
            if (DataModelVersion.MinorVersion >= 184)
            {
                _t = (info.GetValue("R0:T", typeof(Guid?)) as Guid?);
                _y = info.GetBoolean("R0:Y");
                _m = info.GetInt32("R0:M");
            }
            else
            {
                _t = (Guid) info.GetValue("R0:T", typeof(Guid));
            }

            _a = info.GetString("R0:A");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("R0:S", _s);
            info.AddValue("R0:T", _t);
            info.AddValue("R0:A", _a);
            info.AddValue("R0:Y", _y);
            info.AddValue("R0:M", _m);
        }

        private Guid? _s;

        private Guid? _t;

        private string _a;

        private bool _y;

        private int _m;
    }
}
