using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Playfield.CustomLogic.Data;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SPHESC")]
    [Serializable]
    public sealed class SPHESC : BaseAction
    {
        private SPHESC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _e = (PlayerHouseElement)info.GetValue("SPHESC:E", typeof(PlayerHouseElement));
            _v = info.GetString("SPHESC:V");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SPHESC:E", _e);
            info.AddValue("SPHESC:V", _v);
        }

        private PlayerHouseElement _e;

        private string _v;
    }
}
