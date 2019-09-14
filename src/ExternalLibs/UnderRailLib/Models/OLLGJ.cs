using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("OLLGJ")]
    [Serializable]
    public sealed class OLLGJ : Job
    {
        private OLLGJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _lsei = (Guid)info.GetValue("OLLGJ:LSEI", typeof(Guid));
            _rsei = (Guid)info.GetValue("OLLGJ:RSEI", typeof(Guid));
            _an = info.GetString("OLLGJ:AN");
            _o = (SplitGateOperation)info.GetValue("OLLGJ:O", typeof(SplitGateOperation));
            _tf = (info.GetValue("OLLGJ:TF", typeof(int?)) as int?);
            _as = (info.GetValue("OLLGJ:AS", typeof(int?)) as int?);
            _os = info.GetString("OLLGJ:OS");
            _cs = info.GetString("OLLGJ:CS");
            if (DataModelVersion.MinorVersion >= 439)
            {
                _e = info.GetBoolean("OLLGJ:E");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("OLLGJ:LSEI", _lsei);
            info.AddValue("OLLGJ:RSEI", _rsei);
            info.AddValue("OLLGJ:AN", _an);
            info.AddValue("OLLGJ:O", _o);
            info.AddValue("OLLGJ:TF", _tf);
            info.AddValue("OLLGJ:AS", _as);
            info.AddValue("OLLGJ:OS", _os);
            info.AddValue("OLLGJ:CS", _cs);
            info.AddValue("OLLGJ:E", _e);
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
