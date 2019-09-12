using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("OTIGJ")]
    [Serializable]
    public sealed class OTIGJ : Job
    {
        private OTIGJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _lsei = (Guid)info.GetValue("OTIGJ:LSEI", typeof(Guid));
            _rsei = (Guid)info.GetValue("OTIGJ:RSEI", typeof(Guid));
            _an = info.GetString("OTIGJ:AN");
            _o = (SplitGateOperation)info.GetValue("OTIGJ:O", typeof(SplitGateOperation));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("OTIGJ:LSEI", _lsei);
            info.AddValue("OTIGJ:RSEI", _rsei);
            info.AddValue("OTIGJ:AN", _an);
            info.AddValue("OTIGJ:O", _o);
        }

        private const int a = 10;

        private const int b = 200;

        private const int c = 5;

        private const int d = 300;

        private Guid _lsei;

        private Guid _rsei;

        private string _an;

        private SplitGateOperation _o;
    }
}
