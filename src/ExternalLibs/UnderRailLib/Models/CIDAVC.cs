using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CIDAVC")]
    [Serializable]
    public sealed class CIDAVC : Condition
    {
        private CIDAVC(SerializationInfo A_0, StreamingContext A_1) : base(A_0, A_1)
        {
            this._id = (Guid?) A_0.GetValue("CIDAVC:I", typeof(Guid?));
            this._a = A_0.GetString("CIDAVC:A");
            this._n = A_0.GetBoolean("CIDAVC:N");
            this._sf = A_0.GetString("CIDAVC:SF");
            this._vt = (eVT?) A_0.GetValue("CIDAVC:VT", typeof(eVT));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CIDAVC:I", this._id);
            info.AddValue("CIDAVC:A", this._a);
            info.AddValue("CIDAVC:N", this._n);
            info.AddValue("CIDAVC:SF", this._sf);
            info.AddValue("CIDAVC:VT", this._vt);
        }

        private Guid? _id;

        private string _a;

        private bool _n;

        private string _sf;

        private eVT? _vt;
    }
}
