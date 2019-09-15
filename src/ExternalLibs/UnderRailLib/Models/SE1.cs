using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.TimelapseVertigo.Rules.Effects;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SE1")]
    [Serializable]
    public abstract class SE1 : iNM, ISerializable
    {
        protected SE1(SerializationInfo info, StreamingContext ctx)
        {
            _state = (StatusEffectState) info.GetValue("SE1:S", typeof(StatusEffectState));
            _code = info.GetString("SE1:C");
            _name = info.GetString("SE1:N");
            _duration = info.GetInt32("SE1:D");
            _tickPeriod = info.GetInt32("SE1:TP");
            _tc = info.GetDouble("SE1:TC");
            _timeLeft = info.GetDouble("SE1:TL");
            _context = (EffectContext) info.GetValue("SE1:C1", typeof(EffectContext));
            SerializationHelper.ReadList("SE1:SE", ref _staticEffects, info);
            SerializationHelper.ReadList("SE1:DE", ref _dynamicEffects, info);
            _stacking = (eSESB) info.GetValue("SE1:S1", typeof(eSESB));
            _stacks = info.GetInt32("SE1:S2");
            _maxStacks = info.GetInt32("SE1:MS");
            _nature = (eABN) info.GetValue("SE1:N1", typeof(eABN));
            if (DataModelVersion.MinorVersion >= 95)
            {
                _origin = info.GetString("SE1:O");
            }
            else
            {
                object value = info.GetValue("SE1:O", typeof(object));
                if (value != null)
                {
                    _origin = value.ToString();
                }
            }

            _expiredAction = info.GetString("SE1:EA");
            _tickAction = info.GetString("SE1:TA");
            _power = info.GetInt32("SE1:P");
            SerializationHelper.ReadList("SE1:VE", ref _visualEffects, info);
            _icon = info.GetString("SE1:VM");
            _rt = info.GetDouble("SE1:RT");
            if (DataModelVersion.MinorVersion >= 78)
            {
                _silent = info.GetBoolean("SE1:S3");
            }

            if (DataModelVersion.MinorVersion >= 94)
            {
                _appliedAction = info.GetString("SE1:AA");
            }

            if (DataModelVersion.MinorVersion >= 106)
            {
                _initializedJob = info.GetString("SE1:I");
                _uninitializedJob = info.GetString("SE1:U");
            }

            if (DataModelVersion.MinorVersion >= 108)
            {
                _controllerName = info.GetString("SE1:CT");
            }

            if (DataModelVersion.MinorVersion >= 188)
            {
                _removedAction = info.GetString("SE1:RA");
            }

            if (DataModelVersion.MinorVersion >= 222)
            {
                _expiredJob = info.GetString("SE1:EJ");
                _tickJob = info.GetString("SE1:TJ");
                _removedJob = info.GetString("SE1:RJ");
                _appliedJob = info.GetString("SE1:AJ");
            }
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("SE1:S", _state);
            info.AddValue("SE1:C", _code);
            info.AddValue("SE1:N", _name);
            info.AddValue("SE1:D", _duration);
            info.AddValue("SE1:TP", _tickPeriod);
            info.AddValue("SE1:TC", _tc);
            info.AddValue("SE1:TL", _timeLeft);
            info.AddValue("SE1:C1", _context);
            SerializationHelper.WriteList("SE1:SE", _staticEffects, info);
            SerializationHelper.WriteList("SE1:DE", _dynamicEffects, info);
            info.AddValue("SE1:S1", _stacking);
            info.AddValue("SE1:S2", _stacks);
            info.AddValue("SE1:MS", _maxStacks);
            info.AddValue("SE1:N1", _nature);
            info.AddValue("SE1:O", _origin);
            info.AddValue("SE1:EA", _expiredAction);
            info.AddValue("SE1:TA", _tickAction);
            info.AddValue("SE1:RA", _removedAction);
            info.AddValue("SE1:P", _power);
            SerializationHelper.WriteList("SE1:VE", _visualEffects, info);
            info.AddValue("SE1:VM", _icon);
            info.AddValue("SE1:RT", _rt);
            info.AddValue("SE1:S3", _silent);
            info.AddValue("SE1:AA", _appliedAction);
            info.AddValue("SE1:I", _initializedJob);
            info.AddValue("SE1:U", _uninitializedJob);
            info.AddValue("SE1:CT", _controllerName);
            info.AddValue("SE1:EJ", _expiredJob);
            info.AddValue("SE1:TJ", _tickJob);
            info.AddValue("SE1:RJ", _removedJob);
            info.AddValue("SE1:AJ", _appliedJob);
        }

        private StatusEffectState _state;

        private string _code;

        private string _name;

        private int _duration;

        private int _tickPeriod;

        private double _tc;

        private double _timeLeft;

        public double _rt;

        private EffectContext _context;

        private List<SE2> _staticEffects = new List<SE2>();

        private List<DE2> _dynamicEffects = new List<DE2>();

        private eSESB _stacking;

        private int _stacks = 1;

        private int _maxStacks = 1;

        private eABN _nature;

        private string _origin;

        private string _expiredAction;

        private string _tickAction;

        private string _removedAction;

        private string _appliedAction;

        private string _initializedJob;

        private string _uninitializedJob;

        private string _expiredJob;

        private string _tickJob;

        private string _removedJob;

        private string _appliedJob;

        private int _power;

        private List<VisualEffectReference> _visualEffects = new List<VisualEffectReference>();

        private string _icon;

        private bool _silent;

        private string _controllerName;
    }
}
