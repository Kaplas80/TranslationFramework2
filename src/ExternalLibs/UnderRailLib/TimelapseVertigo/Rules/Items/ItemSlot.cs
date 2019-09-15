using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models;
using UnderRailLib.Models.EventArgs;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.TimelapseVertigo.Rules.Items
{
    [EncodedTypeName("IS")]
    [Serializable]
    public class ItemSlot<T> : iITS, ISerializable where T : ItemInstance
    {
        protected ItemSlot(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _item = (T)info.GetValue("IS:I", typeof(T));
                _enabled = info.GetBoolean("IS:E");
                if (DataModelVersion.MinorVersion >= 4)
                {
                    SerializationHelper.ReadEvent("IS:IC", ref c, info);
                }
            }
            else
            {
                if (GetType() == typeof(ItemSlot<T>))
                {
                    _item = (T)info.GetValue("_Item", typeof(T));
                    _enabled = info.GetBoolean("_Enabled");
                    return;
                }
                _item = (T)info.GetValue("ItemSlot`1+_Item", typeof(T));
                _enabled = info.GetBoolean("ItemSlot`1+_Enabled");
            }
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("IS:I", _item);
            info.AddValue("IS:E", _enabled);
            SerializationHelper.WriteEvent("IS:IC", c, info);
        }

        private T _item;

        private bool _enabled = true;

        private EventHandler<eaPCEA> c;
    }
}
