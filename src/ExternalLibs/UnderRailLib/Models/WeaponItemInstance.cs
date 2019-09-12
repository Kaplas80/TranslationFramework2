using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("WII")]
    [Serializable]
    public sealed class WeaponItemInstance : EquippableItemInstance, IItemInstance<WeaponItem>, ISerializable
    {
        private WeaponItemInstance(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion >= 51)
            {
                SerializationHelper.ReadList("WII:A", ref _a, info);
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            SerializationHelper.WriteList("WII:A", _a, info);
        }

        private List<AmmoType> _a = new List<AmmoType>();
    }
}