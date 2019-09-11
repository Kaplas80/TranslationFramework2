using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Data;

namespace UnderRailLib.Models
{
    [EncodedTypeName("II")]
    [Serializable]
    public abstract class II : iID, iITI, ISerializable
    {
        protected II(SerializationInfo info, StreamingContext ctx)
        {
            _id = (Guid) info.GetValue("II:I", typeof(Guid));
            _stacks = info.GetInt32("II:S");
            _definitionProvider = (ItemDefinitionProvider) info.GetValue("II:DP", typeof(ItemDefinitionProvider));
            _lootable = info.GetBoolean("II:L");
            _customProperties = (PropertyCollection) info.GetValue("II:CP", typeof(PropertyCollection));
            if (_customProperties == null)
            {
                _customProperties = new PropertyCollection();
            }

            _stealable = info.GetBoolean("II:S1");

            if (DataModelVersion.MinorVersion >= 30)
            {
                _battery = info.GetDouble("II:B");
                _maxBatteryLegacy = info.GetInt32("II:MB");
            }

            if (DataModelVersion.MinorVersion >= 145)
            {
                _durability = info.GetDouble("II:D");
            }

            if (DataModelVersion.MinorVersion >= 510)
            {
                _version = (Version) info.GetValue("II:V", typeof(Version));
            }
            else
            {
                _version = new Version(0, 0, 0, 0);
            }

            if (DataModelVersion.MinorVersion >= 540)
            {
                SerializationHelper.ReadList("II:F", ref _itemEffects, info);
                return;
            }

            _itemEffects = new List<ItemEffects>();
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("II:I", _id);
            info.AddValue("II:S", _stacks);
            info.AddValue("II:DP", _definitionProvider);
            info.AddValue("II:L", _lootable);
            info.AddValue("II:CP", _customProperties);
            info.AddValue("II:S1", _stealable);
            if (DataModelVersion.MinorVersion >= 30)
            {
                info.AddValue("II:B", _battery);
                info.AddValue("II:MB", _maxBatteryLegacy);
            }

            if (DataModelVersion.MinorVersion >= 145)
            {
                info.AddValue("II:D", _durability);
            }

            if (DataModelVersion.MinorVersion >= 510)
            {
                info.AddValue("II:V", _version);
            }

            if (DataModelVersion.MinorVersion >= 540)
            {
                SerializationHelper.WriteList("II:F", _itemEffects, info);
            }
        }
        
        private Guid _id;

        private int _stacks = 1;

        private ItemDefinitionProvider _definitionProvider;

        private bool _lootable = true;

        private PropertyCollection _customProperties = new PropertyCollection();

        private bool _stealable = true;

        private double _battery;

        private int _maxBatteryLegacy;

        private double _durability;

        private Version _version = new Version(1, 1, 0, 12);

        private List<ItemEffects> _itemEffects = new List<ItemEffects>();
    }
}