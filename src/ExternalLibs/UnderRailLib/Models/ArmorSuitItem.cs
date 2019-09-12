using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ASI")]
    [Serializable]
    public sealed class ArmorSuitItem : ArmorItem
    {
        private ArmorSuitItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _armorSuitVisualModel = info.GetString("ASI:ASVM");
                if (DataModelVersion.MinorVersion >= 117)
                {
                    _c = (info.GetValue("ASI:C", typeof(eCMC?)) as eCMC?);
                }

                if (DataModelVersion.MinorVersion >= 163)
                {
                    _o = info.GetBoolean("ASI:O");
                }

                if (DataModelVersion.MinorVersion >= 164)
                {
                    _bc = info.GetDouble("ASI:BC");
                    _ba = info.GetInt32("ASI:BA");
                }
            }
            else
            {
                if (GetType() == typeof(ArmorSuitItem))
                {
                    _armorSuitVisualModel = info.GetString("_ArmorSuitVisualModel");
                    return;
                }

                _armorSuitVisualModel = info.GetString("ArmorSuitItem+_ArmorSuitVisualModel");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("ASI:ASVM", _armorSuitVisualModel);
            info.AddValue("ASI:C", _c);
            info.AddValue("ASI:O", _o);
            info.AddValue("ASI:BC", _bc);
            info.AddValue("ASI:BA", _ba);
        }

        private string _armorSuitVisualModel;

        private eCMC? _c;

        private bool _o;

        private double _bc;

        private int _ba;
    }
}