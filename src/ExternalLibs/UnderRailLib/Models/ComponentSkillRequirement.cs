using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CSR")]
    [Serializable]
    public sealed class ComponentSkillRequirement : ISerializable
    {
        private ComponentSkillRequirement(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _skillName = info.GetString("CSR:SN");
                _factor = info.GetSingle("CSR:F");
                return;
            }

            if (GetType() == typeof(ComponentSkillRequirement))
            {
                _skillName = info.GetString("_SkillName");
                _factor = info.GetSingle("_Factor");
                return;
            }

            _skillName = info.GetString("ComponentSkillRequirement+_SkillName");
            _factor = info.GetSingle("ComponentSkillRequirement+_Factor");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("CSR:SN", _skillName);
            info.AddValue("CSR:F", _factor);
        }

        private string _skillName;

        private float _factor;
    }
}