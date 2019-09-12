using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("AI2")]
    [Serializable]
    public sealed class AmmoItem : NonEquippableItem
    {
        private AmmoItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _t = (AmmoType) info.GetValue("AI2:T", typeof(AmmoType));
            if (DataModelVersion.MinorVersion >= 261)
            {
                SerializationHelper.ReadList("AI2:H", ref _h, info);
                return;
            }

            _h = new List<HitEffectReference>();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("AI2:T", _t);
            SerializationHelper.WriteList("AI2:H", _h, info);
        }

        private AmmoType _t;

        private List<HitEffectReference> _h = new List<HitEffectReference>();
    }
}
