using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("R01")]
    [Serializable]
    public sealed class R01 : BaseAction
    {
        private R01(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _m = info.GetInt32("R01:M");
            if (DataModelVersion.MajorVersion >= 545)
            {
                _p = info.GetString("R02:P");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("R01:M", _m);
            info.AddValue("R02:P", _p);
        }
        
        private int _m;
        private string _p;
    }
}
