using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("I01")]
    [Serializable]
    public sealed class ImmuneEffect : StatusEffect, iMS
    {
        private ImmuneEffect(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (eCAI)info.GetValue("I01:I", typeof(eCAI));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("I01:I", _i);
        }

        private eCAI _i;
    }
}
