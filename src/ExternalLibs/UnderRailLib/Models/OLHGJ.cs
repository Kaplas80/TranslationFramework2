using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("OLHGJ")]
    [Serializable]
    public sealed class OLHGJ : Job
    {
        private OLHGJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _lsei = (Guid)info.GetValue("OLHGJ:LSEI", typeof(Guid));
            _rsei = (Guid)info.GetValue("OLHGJ:RSEI", typeof(Guid));
            _an = info.GetString("OLHGJ:AN");
            _o = (SplitGateOperation)info.GetValue("OLHGJ:O", typeof(SplitGateOperation));
            _tf = (info.GetValue("OLHGJ:TF", typeof(int?)) as int?);
            _as = (info.GetValue("OLHGJ:AS", typeof(int?)) as int?);
            _os = info.GetString("OLHGJ:OS");
            _cs = info.GetString("OLHGJ:CS");
            if (DataModelVersion.MinorVersion >= 439)
            {
                _e = info.GetBoolean("OLHGJ:E");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("OLHGJ:LSEI", _lsei);
            info.AddValue("OLHGJ:RSEI", _rsei);
            info.AddValue("OLHGJ:AN", _an);
            info.AddValue("OLHGJ:O", _o);
            info.AddValue("OLHGJ:TF", _tf);
            info.AddValue("OLHGJ:AS", _as);
            info.AddValue("OLHGJ:OS", _os);
            info.AddValue("OLHGJ:CS", _cs);
            info.AddValue("OLHGJ:E", _e);
        }

        private Guid _lsei;

        private Guid _rsei;

        private string _an;

        private SplitGateOperation _o;

        private int? _tf;

        private int? _as;

        private string _os;

        private string _cs;

        private bool _e;
    }
}
