using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("EI")]
    [Serializable]
    public abstract class EquippableItem : Item, iRQR
    {
        protected EquippableItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                SerializationHelper.ReadList("EI:SE", ref _staticEffects, info);
                if (DataModelVersion.MinorVersion >= 113)
                {
                    _e = info.GetBoolean("EI:E");
                }

                if (DataModelVersion.MinorVersion >= 132)
                {
                    SerializationHelper.ReadList("EI:DE", ref _de, info);
                }
                else
                {
                    _de = new List<DE2>();
                }

                if (DataModelVersion.MinorVersion >= 365)
                {
                    SerializationHelper.ReadList("EI:QC", ref _qc, info);
                }
                else
                {
                    _qc = new List<CapabilityReference>();
                }

                if (DataModelVersion.MinorVersion >= 444)
                {
                    _lc = (info.GetValue("EI:LC", typeof(Color?)) as Color?);
                    _li = info.GetSingle("EI:LI");
                    _lfr = info.GetSingle("EI:LFR");
                    _lr = info.GetSingle("EI:LR");
                    _ln = info.GetBoolean("EI:LN");
                }

                if (DataModelVersion.MinorVersion >= 452)
                {
                    SerializationHelper.ReadList("EI:RQ", ref _rq, info);
                    return;
                }

                _rq = new List<RQX>();
            }
            else
            {
                if (GetType() == typeof(EquippableItem))
                {
                    _staticEffects = (List<StatusEffect>) info.GetValue("_StaticEffects", typeof(List<StatusEffect>));
                    return;
                }

                _staticEffects = (List<StatusEffect>) info.GetValue("EquippableItem+_StaticEffects", typeof(List<StatusEffect>));
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            SerializationHelper.WriteList("EI:SE", _staticEffects, info);
            if (DataModelVersion.MinorVersion >= 132)
            {
                SerializationHelper.WriteList("EI:DE", _de, info);
            }

            if (DataModelVersion.MinorVersion >= 113)
            {
                info.AddValue("EI:E", _e);
            }

            if (DataModelVersion.MinorVersion >= 365)
            {
                SerializationHelper.WriteList("EI:QC", _qc, info);
            }

            if (DataModelVersion.MinorVersion >= 444)
            {
                info.AddValue("EI:LC", _lc);
                info.AddValue("EI:LI", _li);
                info.AddValue("EI:LFR", _lfr);
                info.AddValue("EI:LR", _lr);
                info.AddValue("EI:LN", _ln);
            }

            if (DataModelVersion.MinorVersion >= 452)
            {
                SerializationHelper.WriteList("EI:RQ", _rq, info);
            }
        }

        private List<StatusEffect> _staticEffects = new List<StatusEffect>();

        private List<DE2> _de = new List<DE2>();

        private bool _e;

        protected List<CapabilityReference> _qc = new List<CapabilityReference>();

        private Color? _lc;

        private float _li;

        private float _lfr = 0.05f;

        private float _lr;

        private bool _ln;

        private List<RQX> _rq = new List<RQX>();
    }
}