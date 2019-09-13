using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SCOM")]
    [Serializable]
    public sealed class SCOM : Job
    {
        private SCOM(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _wn = info.GetString("SCOM:WN");
            _tid = (Guid?) info.GetValue("SCOM:TID", typeof(Guid?));
            _ch = (eSPCJBT) info.GetValue("SCOM:CH", typeof(eSPCJBT));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SCOM:WN", _wn);
            info.AddValue("SCOM:TID", _tid);
            info.AddValue("SCOM:CH", _ch);
        }

        private string _wn;

        private Guid? _tid;

        private eSPCJBT _ch = eSPCJBT.c;
    }
}
