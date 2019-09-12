using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CI")]
    [Serializable]
    public class ComponentItem : Item
    {
        protected ComponentItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                SerializationHelper.ReadList("CI:SR", ref _skillRequirements, info);
                SerializationHelper.ReadList("CI:SB", ref _skillBonuses, info);
                _componentType = (ComponentType) info.GetValue("CI:CT", typeof(ComponentType));
                if (DataModelVersion.MinorVersion >= 23)
                {
                    _bv = info.GetInt32("CI:BV");
                    _avpq = info.GetDouble("CI:AVPQ");
                }
            }
            else
            {
                if (GetType() == typeof(ComponentItem))
                {
                    _skillRequirements = (List<ComponentSkillRequirement>) info.GetValue("_SkillRequirements", typeof(List<ComponentSkillRequirement>));
                    _skillBonuses = (List<GenericComponentModifier>) info.GetValue("_SkillBonuses", typeof(List<GenericComponentModifier>));
                    _componentType = (ComponentType) info.GetValue("_ComponentType", typeof(ComponentType));
                    return;
                }

                _skillRequirements = (List<ComponentSkillRequirement>) info.GetValue("ComponentItem+_SkillRequirements", typeof(List<ComponentSkillRequirement>));
                _skillBonuses = (List<GenericComponentModifier>) info.GetValue("ComponentItem+_SkillBonuses", typeof(List<GenericComponentModifier>));
                _componentType = (ComponentType) info.GetValue("ComponentItem+_ComponentType", typeof(ComponentType));
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            SerializationHelper.WriteList("CI:SR", _skillRequirements, info);
            SerializationHelper.WriteList("CI:SB", _skillBonuses, info);
            info.AddValue("CI:CT", _componentType);
            info.AddValue("CI:BV", _bv);
            info.AddValue("CI:AVPQ", _avpq);
        }

        private List<ComponentSkillRequirement> _skillRequirements = new List<ComponentSkillRequirement>();

        private List<GenericComponentModifier> _skillBonuses = new List<GenericComponentModifier>();

        private ComponentType _componentType;

        private int _bv;

        private double _avpq;
    }
}
