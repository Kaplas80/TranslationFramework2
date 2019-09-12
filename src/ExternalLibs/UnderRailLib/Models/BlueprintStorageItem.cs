using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("BSI")]
    [Serializable]
    public sealed class BlueprintStorageItem : NonEquippableItem, ISerializable
    {
        private BlueprintStorageItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _blueprintName = info.GetString("BSI:BN");
                return;
            }

            if (GetType() == typeof(BlueprintStorageItem))
            {
                _blueprintName = info.GetString("_BlueprintName");
                return;
            }

            _blueprintName = info.GetString("BlueprintStorageItem+_BlueprintName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("BSI:BN", _blueprintName);
        }

        private string _blueprintName;
    }
}