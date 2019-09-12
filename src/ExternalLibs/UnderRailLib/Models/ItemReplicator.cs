using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("IR")]
    [Serializable]
    public sealed class ItemReplicator : ItemGeneratorBase
    {
        private ItemReplicator(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _item = (ItemInstance) info.GetValue("IR:I", typeof(ItemInstance));
                _alwaysMakeIndependentDefinition = info.GetBoolean("IR:AMID");
                if (DataModelVersion.MinorVersion >= 20)
                {
                    _trq = info.GetBoolean("IR:TRQ");
                }
            }
            else
            {
                if (GetType() == typeof(ItemReplicator))
                {
                    _item = (ItemInstance) info.GetValue("_Item", typeof(ItemInstance));
                    _alwaysMakeIndependentDefinition = info.GetBoolean("_AlwaysMakeIndependentDefinition");
                    return;
                }

                _item = (ItemInstance) info.GetValue("ItemReplicator+_Item", typeof(ItemInstance));
                _alwaysMakeIndependentDefinition = info.GetBoolean("ItemReplicator+_AlwaysMakeIndependentDefinition");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("IR:I", _item);
            info.AddValue("IR:AMID", _alwaysMakeIndependentDefinition);
            info.AddValue("IR:TRQ", _trq);
        }

        private ItemInstance _item;

        private bool _alwaysMakeIndependentDefinition;

        private bool _trq;
    }
}