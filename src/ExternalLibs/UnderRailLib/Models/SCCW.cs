using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Characters;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SCCW")]
    [Serializable]
    public sealed class SCCW : Job
    {
        private SCCW(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?) info.GetValue("SCCW:I", typeof(Guid?));
            _a = info.GetString("SCCW:A");
            _w = (WeaponKey)info.GetValue("SCCW:W", typeof(WeaponKey));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SCCW:I", _i);
            info.AddValue("SCCW:A", _a);
            info.AddValue("SCCW:W", _w);
        }

        private WeaponKey _w;

        private Guid? _i;

        private string _a;
    }
}
