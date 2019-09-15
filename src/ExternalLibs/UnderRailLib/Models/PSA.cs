using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Aspects;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PSA")]
    [Serializable]
    public sealed class PSA : Aspect<LE2>, iINA, dfi
    {
        private PSA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _e = info.GetBoolean("PSA:E");
            _ef = info.GetBoolean("PSA:EF");
            _pl = info.GetBoolean("PSA:PL");
            _psn = info.GetString("PSA:PSN");
            if (DataModelVersion.MinorVersion >= 38)
            {
                _d = info.GetBoolean("PSA:D");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PSA:E", _e);
            info.AddValue("PSA:EF", _ef);
            info.AddValue("PSA:PL", _pl);
            info.AddValue("PSA:PSN", _psn);
            info.AddValue("PSA:D", _d);
        }

        private bool _e;

        private bool _ef;

        private bool _pl = true;

        private string _psn;

        private bool _d;
    }
}
