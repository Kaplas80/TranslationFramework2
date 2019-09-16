using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Ouroboros.Common.Aspects;
using UnderRailLib.TimelapseVertigo.Playfield.Locale;

namespace UnderRailLib.Models
{
    [EncodedTypeName("LEA")]
    [Serializable]
    public sealed class LockEntityAspect : Aspect<LE2>
    {
        private LockEntityAspect(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion == 0)
            {
                _locked = info.GetBoolean("_Locked");
                _lockType = (LockType) info.GetValue("_LockType", typeof(LockType));
                _lockpickable = info.GetBoolean("_Lockpickable");
                _requiredLockpickingSkill = info.GetInt32("_RequiredLockpickingSkill");
                _keyCode = info.GetString("_KeyCode");
                _consumesKey = info.GetBoolean("_ConsumesKey");
                _relockable = info.GetBoolean("_Relockable");
                _experienceModifier = info.GetDouble("_ExperienceModifier");
                try
                {
                    _lpm = (eLOPMO) info.GetValue("LEA_LPM", typeof(eLOPMO));
                }
                catch
                {
                }

                SerializationHelper.ReadList("_OnLockedJobs", ref _onLockedJobs, info);
                SerializationHelper.ReadList("_OnUnlockedJobs", ref _onUnlockedJobs, info);
                SerializationHelper.ReadList("_OnLockpickedJobs", ref _onLockpickedJobs, info);
                return;
            }

            _locked = info.GetBoolean("LEA:L");
            _lockType = (LockType) info.GetValue("LEA:LT", typeof(LockType));
            _lockpickable = info.GetBoolean("LEA:L1");
            _requiredLockpickingSkill = info.GetInt32("LEA:RLS");
            _keyCode = info.GetString("LEA:KC");
            _consumesKey = info.GetBoolean("LEA:CK");
            _relockable = info.GetBoolean("LEA:R");
            _experienceModifier = info.GetDouble("LEA:EM");
            SerializationHelper.ReadList("LEA:OUJ", ref _onUnlockedJobs, info);
            SerializationHelper.ReadList("LEA:OLJ", ref _onLockedJobs, info);
            SerializationHelper.ReadList("LEA:OLJ1", ref _onLockpickedJobs, info);
            _lpm = (eLOPMO) info.GetValue("LEA:LPM", typeof(eLOPMO));
            if (DataModelVersion.MinorVersion >= 227)
            {
                h = info.GetBoolean("LEA:P");
            }

            if (DataModelVersion.MinorVersion >= 249)
            {
                o = info.GetString("LEA:C");
            }

            if (DataModelVersion.MinorVersion >= 492)
            {
                SerializationHelper.ReadList("LEA:LPCJ", ref m, info);
                return;
            }

            m = new List<Job>();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("LEA:L", _locked);
            info.AddValue("LEA:LT", _lockType);
            info.AddValue("LEA:L1", _lockpickable);
            info.AddValue("LEA:RLS", _requiredLockpickingSkill);
            info.AddValue("LEA:KC", _keyCode);
            info.AddValue("LEA:CK", _consumesKey);
            info.AddValue("LEA:R", _relockable);
            info.AddValue("LEA:EM", _experienceModifier);
            SerializationHelper.WriteList("LEA:OUJ", _onUnlockedJobs, info);
            SerializationHelper.WriteList("LEA:OLJ", _onLockedJobs, info);
            SerializationHelper.WriteList("LEA:OLJ1", _onLockpickedJobs, info);
            SerializationHelper.WriteList("LEA:LPCJ", m, info);
            info.AddValue("LEA:LPM", _lpm);
            info.AddValue("LEA:P", h);
            info.AddValue("LEA:C", o);
        }

        private bool _locked;

        private LockType _lockType;

        private bool _lockpickable = true;

        private int _requiredLockpickingSkill;

        private string _keyCode;

        private bool _consumesKey;

        private bool _relockable;

        private bool h;

        private double _experienceModifier = 1.0;

        private List<Job> _onUnlockedJobs = new List<Job>();

        private List<Job> _onLockedJobs = new List<Job>();

        private List<Job> _onLockpickedJobs = new List<Job>();

        private List<Job> m = new List<Job>();

        private eLOPMO _lpm;

        private string o;
    }
}
