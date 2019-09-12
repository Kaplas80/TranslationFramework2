using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("R02")]
    [Serializable]
    public sealed class R02 : Condition
    {
        private R02(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _m = info.GetInt32("R02:M");
            _x = info.GetInt32("R02:X");
            if (DataModelVersion.MajorVersion >= 545)
            {
                _p = info.GetString("R02:P");
                _d = info.GetBoolean("R02:D");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("R02:M", _m);
            info.AddValue("R02:X", _x);
            info.AddValue("R02:P", _p);
            info.AddValue("R02:D", _d);
        }

        private int _m;

        private int _x;

        private string _p;

        private bool _d;
    }
}
