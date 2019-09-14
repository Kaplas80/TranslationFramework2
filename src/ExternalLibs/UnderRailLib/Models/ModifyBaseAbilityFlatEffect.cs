using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MBAFE")]
    [Serializable]
    public sealed class ModifyBaseAbilityFlatEffect : ModifyStatFlatEffect, iBAFX
    {
        private ModifyBaseAbilityFlatEffect(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _baseAbilityName = info.GetString("MBAFE:BAN");
                return;
            }
            if (GetType() == typeof(ModifyBaseAbilityFlatEffect))
            {
                _baseAbilityName = info.GetString("_BaseAbilityName");
                return;
            }
            _baseAbilityName = info.GetString("ModifyBaseAbilityFlatEffect+_BaseAbilityName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MBAFE:BAN", _baseAbilityName);
        }

        private string _baseAbilityName;
    }
}
