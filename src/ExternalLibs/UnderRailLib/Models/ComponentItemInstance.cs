using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CII")]
    [Serializable]
    public sealed class ComponentItemInstance : NonEquippableItemInstance, IItemInstance<ComponentItem>, iQLII
    {
        private ComponentItemInstance(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _qualityLevel = info.GetInt32("CII:QL");
                return;
            }

            if (GetType() == typeof(ComponentItemInstance))
            {
                _qualityLevel = info.GetInt32("_QualityLevel");
                return;
            }

            _qualityLevel = info.GetInt32("ComponentItemInstance+_QualityLevel");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CII:QL", _qualityLevel);
        }

       private int _qualityLevel;
    }
}
