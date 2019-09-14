using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PHECTPVC")]
    [Serializable]
    public sealed class PHECTPVC : Condition
    {
        private PHECTPVC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = info.GetBoolean("PHECTPVC:I");
            _c = info.GetString("PHECTPVC:C");
            _d = info.GetInt32("PHECTPVC:D");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PHECTPVC:I", _i);
            info.AddValue("PHECTPVC:C", _c);
            info.AddValue("PHECTPVC:D", _d);
        }

        private bool _i;

        private string _c;

        private int _d;
    }
}