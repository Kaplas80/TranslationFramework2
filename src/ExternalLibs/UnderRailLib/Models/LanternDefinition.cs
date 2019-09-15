using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Data;

namespace UnderRailLib.Models
{
    [EncodedTypeName("LD")]
    [Serializable]
    public sealed class LanternDefinition : iID, iNM, iIC, iLY, ISerializable
    {
        private LanternDefinition(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _id = (Guid) info.GetValue("LD:I1", typeof(Guid));
                _name = info.GetString("LD:N");
                _layer = info.GetInt32("LD:L");
                _enabled = info.GetBoolean("LD:E");
                _light = (Light) info.GetValue("LD:L1", typeof(Light));
                _lightFadeRate = info.GetSingle("LD:LFR");
                _range = info.GetSingle("LD:R");
                SerializationHelper.ReadList("LD:APP", ref _additionalPenetrationPredicates, info);
                SerializationHelper.ReadList("LD:AIP", ref _additionalImpactPredicates, info);
                SerializationHelper.ReadList("LD:I2", ref _instances, info);
                _zone = (Zone) info.GetValue("LD:Z", typeof(Zone));
                return;
            }

            if (GetType() == typeof(LanternDefinition))
            {
                _id = (Guid) info.GetValue("_Id", typeof(Guid));
                _name = info.GetString("_Name");
                _layer = info.GetInt32("_Layer");
                _enabled = info.GetBoolean("_Enabled");
                _light = (Light) info.GetValue("_Light", typeof(Light));
                _lightFadeRate = info.GetSingle("_LightFadeRate");
                _range = info.GetSingle("_Range");
                _additionalPenetrationPredicates = (List<ExternalResolverReference<iPEP>>) info.GetValue("_AdditionalPenetrationPredicates",
                    typeof(List<ExternalResolverReference<iPEP>>));
                _additionalImpactPredicates = (List<ExternalResolverReference<iIMP>>) info.GetValue("_AdditionalImpactPredicates",
                    typeof(List<ExternalResolverReference<iIMP>>));
                _instances = (List<Lantern>) info.GetValue("_Instances", typeof(List<Lantern>));
                _zone = (Zone) info.GetValue("_Zone", typeof(Zone));
                return;
            }

            _id = (Guid) info.GetValue("LanternDefinition+_Id", typeof(Guid));
            _name = info.GetString("LanternDefinition+_Name");
            _layer = info.GetInt32("LanternDefinition+_Layer");
            _enabled = info.GetBoolean("LanternDefinition+_Enabled");
            _light = (Light) info.GetValue("LanternDefinition+_Light", typeof(Light));
            _lightFadeRate = info.GetSingle("LanternDefinition+_LightFadeRate");
            _range = info.GetSingle("LanternDefinition+_Range");
            _additionalPenetrationPredicates = (List<ExternalResolverReference<iPEP>>) info.GetValue(
                "LanternDefinition+_AdditionalPenetrationPredicates", typeof(List<ExternalResolverReference<iPEP>>));
            _additionalImpactPredicates = (List<ExternalResolverReference<iIMP>>) info.GetValue(
                "LanternDefinition+_AdditionalImpactPredicates", typeof(List<ExternalResolverReference<iIMP>>));
            _instances = (List<Lantern>) info.GetValue("LanternDefinition+_Instances", typeof(List<Lantern>));
            _zone = (Zone) info.GetValue("LanternDefinition+_Zone", typeof(Zone));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("LD:I1", _id);
            info.AddValue("LD:N", _name);
            info.AddValue("LD:L", _layer);
            info.AddValue("LD:E", _enabled);
            info.AddValue("LD:L1", _light);
            info.AddValue("LD:LFR", _lightFadeRate);
            info.AddValue("LD:R", _range);
            SerializationHelper.WriteList("LD:APP", _additionalPenetrationPredicates, info);
            SerializationHelper.WriteList("LD:AIP", _additionalImpactPredicates, info);
            SerializationHelper.WriteList("LD:I2", _instances, info);
            info.AddValue("LD:Z", _zone);
        }

        private Guid _id;

        private string _name;

        private int _layer;

        private bool _enabled = true;

        private Light _light;

        private float _lightFadeRate = 0.05f;

        private float _range;

        private List<ExternalResolverReference<iPEP>> _additionalPenetrationPredicates = new List<ExternalResolverReference<iPEP>>();

        private List<ExternalResolverReference<iIMP>> _additionalImpactPredicates = new List<ExternalResolverReference<iIMP>>();

        private List<Lantern> _instances = new List<Lantern>();

        private Zone _zone;
    }
}
