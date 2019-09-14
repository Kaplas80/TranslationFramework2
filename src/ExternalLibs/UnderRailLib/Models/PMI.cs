using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PMI")]
    [Serializable]
    public sealed class PMI : NonEquippableItem, iRQR
    {
        private PMI(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _p = info.GetString("PMI:P");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PMI:P", _p);
        }

        private string _p;
    }
}
