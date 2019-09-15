using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MQSNE")]
    [Serializable]
    public sealed class ModifyQuickSlotsNumberEffect : StatusEffect
    {
        private ModifyQuickSlotsNumberEffect(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _numberOfSlots = info.GetInt32("MQSNE:NOS");
                return;
            }
            if (GetType() == typeof(ModifyQuickSlotsNumberEffect))
            {
                _numberOfSlots = info.GetInt32("_NumberOfSlots");
                return;
            }
            _numberOfSlots = info.GetInt32("ModifyQuickSlotsNumberEffect+_NumberOfSlots");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MQSNE:NOS", _numberOfSlots);
        }

        private int _numberOfSlots;
    }
}
