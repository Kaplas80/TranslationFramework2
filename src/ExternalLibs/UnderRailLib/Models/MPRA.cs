using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MPRA")]
    [Serializable]
    public sealed class MPRA : BaseAction
    {
        private MPRA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _v = info.GetSingle("MPRA:V");
            _ll = (info.GetValue("MPRA:LL", typeof(float?)) as float?);
            _ul = (info.GetValue("MPRA:UL", typeof(float?)) as float?);
            _i = info.GetBoolean("MPRA:I");
            _r = (CRP) info.GetValue("MPRA:R", typeof(CRP));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MPRA:V", _v);
            info.AddValue("MPRA:LL", _ll);
            info.AddValue("MPRA:UL", _ul);
            info.AddValue("MPRA:I", _i);
            info.AddValue("MPRA:R", _r);
        }

        private float? _ll = -2f;

        private float? _ul = 2f;

        private float _v;

        private CRP _r;

        private bool _i;
    }
}
