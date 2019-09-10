using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("NEI")]
    [Serializable]
    public class NonEquippableItem : Item
    {
        protected NonEquippableItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _combatReady = info.GetBoolean("NEI:CR");
                return;
            }
            if (GetType() == typeof(NonEquippableItem))
            {
                _combatReady = info.GetBoolean("_CombatReady");
                return;
            }
            _combatReady = info.GetBoolean("NonEquippableItem+_CombatReady");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("NEI:CR", _combatReady);
        }

        private bool _combatReady;
    }

}
