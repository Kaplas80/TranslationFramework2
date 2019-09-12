using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("AI1")]
    [Serializable]
    public abstract class ArmorItem : EquippableItem, ISerializable
    {
        protected ArmorItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                SerializationHelper.ReadList("AI1:DR", ref _dr, info);
                _e = DataModelVersion.MinorVersion >= 91 ? info.GetDouble("AI1:E") : info.GetSingle("AI1:E");

                if (DataModelVersion.MinorVersion >= 90)
                {
                    SerializationHelper.ReadList("AT1:M", ref _m, info);
                }
                else
                {
                    _m = new List<AIM>();
                }

                if (DataModelVersion.MinorVersion >= 96)
                {
                    _s = info.GetInt32("AT1:S");
                }
            }
            else
            {
                if (GetType() == typeof(ArmorItem))
                {
                    _dr = (List<DR2>) info.GetValue("_DamageResistances", typeof(List<DR2>));
                    _e = info.GetSingle("_Encumbrance");
                    return;
                }

                _dr = (List<DR2>) info.GetValue("ArmorItem+_DamageResistances", typeof(List<DR2>));
                _e = info.GetSingle("ArmorItem+_Encumbrance");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            SerializationHelper.WriteList("AI1:DR", _dr, info);
            info.AddValue("AI1:E", _e);
            SerializationHelper.WriteList("AT1:M", _m, info);
            info.AddValue("AT1:S", _s);
        }

        private List<DR2> _dr = new List<DR2>();

        private double _e;

        private List<AIM> _m = new List<AIM>();

        private int _s;
    }
}