using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CIDAVC")]
    [Serializable]
    public sealed class CIDAVC : Condition
    {
        private CIDAVC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _id = (Guid?) info.GetValue("CIDAVC:I", typeof(Guid?));
            _a = info.GetString("CIDAVC:A");
            _n = info.GetBoolean("CIDAVC:N");
            _sf = info.GetString("CIDAVC:SF");
            _vt = (eVT?) info.GetValue("CIDAVC:VT", typeof(eVT));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CIDAVC:I", _id);
            info.AddValue("CIDAVC:A", _a);
            info.AddValue("CIDAVC:N", _n);
            info.AddValue("CIDAVC:SF", _sf);
            info.AddValue("CIDAVC:VT", _vt);
        }

        private Guid? _id;

        private string _a;

        private bool _n;

        private string _sf;

        private eVT? _vt;
    }
}
