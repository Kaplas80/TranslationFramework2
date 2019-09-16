using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DEA")]
    [Serializable]
    public class DEA : OCUEA
    {
        protected DEA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _ctfm = (CS) info.GetValue("DEA:CTFM", typeof(CS));
            if (DataModelVersion.MinorVersion >= 44)
            {
                _c = (info.GetValue("DEA:C", typeof(int?)) as int?);
                _o = (info.GetValue("DEA:O", typeof(int?)) as int?);
            }

            if (DataModelVersion.MinorVersion >= 71)
            {
                _l = (info.GetValue("DEA:L", typeof(Guid?)) as Guid?);
            }

            if (DataModelVersion.MinorVersion >= 354)
            {
                _s = info.GetBoolean("DEA:S");
                return;
            }

            _s = true;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("DEA:CTFM", _ctfm);
            info.AddValue("DEA:C", _c);
            info.AddValue("DEA:O", _o);
            info.AddValue("DEA:L", _l);
            info.AddValue("DEA:S", _s);
        }

        private CS _ctfm;

        public int? _c;

        public int? _o;

        private Guid? _l;

        private bool _s = true;
    }
}
