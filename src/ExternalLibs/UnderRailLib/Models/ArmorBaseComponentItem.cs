using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ABCI")]
    [Serializable]
    public class ArmorBaseComponentItem : ComponentItem
    {
        protected ArmorBaseComponentItem(SerializationInfo A_0, StreamingContext A_1) : base(A_0, A_1)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                SerializationHelper.ReadList("ABCI:DRF", ref _damageResistanceFactors, A_0);
                if (DataModelVersion.MinorVersion >= 91)
                {
                    _encumbrance = A_0.GetDouble("ABCI:E");
                }
                else
                {
                    _encumbrance = A_0.GetSingle("ABCI:E");
                }
                if (DataModelVersion.MinorVersion >= 90)
                {
                    SerializationHelper.ReadList("ABCI:M", ref _m, A_0);
                }
                else
                {
                    _m = new List<AIM>();
                }
                if (DataModelVersion.MinorVersion >= 96)
                {
                    _s = A_0.GetInt32("ABCI:S");
                }
                if (DataModelVersion.MinorVersion >= 187)
                {
                    SerializationHelper.ReadList("ABCI:SE", ref _se, A_0);
                    SerializationHelper.ReadList("ABCI:DE", ref _de, A_0);
                    return;
                }
                _se = new List<SE2>();
                _de = new List<DE2>();
            }
            else
            {
                if (GetType() == typeof(ArmorBaseComponentItem))
                {
                    _damageResistanceFactors = (List<DTF>)A_0.GetValue("_DamageResistanceFactors", typeof(List<DTF>));
                    _encumbrance = A_0.GetSingle("_Encumbrance");
                    return;
                }
                _damageResistanceFactors = (List<DTF>)A_0.GetValue("ArmorBaseComponentItem+_DamageResistanceFactors", typeof(List<DTF>));
                _encumbrance = A_0.GetSingle("ArmorBaseComponentItem+_Encumbrance");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            SerializationHelper.WriteList("ABCI:DRF", _damageResistanceFactors, info);
            info.AddValue("ABCI:E", _encumbrance);
            SerializationHelper.WriteList("ABCI:M", _m, info);
            info.AddValue("ABCI:S", _s);
            SerializationHelper.WriteList("ABCI:SE", _se, info);
            SerializationHelper.WriteList("ABCI:DE", _de, info);
        }

        private List<DTF> _damageResistanceFactors = new List<DTF>();

        private double _encumbrance;

        private List<AIM> _m = new List<AIM>();

        private int _s;

        private List<SE2> _se = new List<SE2>();

        private List<DE2> _de = new List<DE2>();
    }
}
