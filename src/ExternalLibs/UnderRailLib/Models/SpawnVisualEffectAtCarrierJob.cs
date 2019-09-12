using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SVEACJ")]
    [Serializable]
    public class SpawnVisualEffectAtCarrierJob : Job
    {
        private SpawnVisualEffectAtCarrierJob(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _effectModel = info.GetString("SVEACJ:EM");
                _attachToCarrier = info.GetBoolean("SVEACJ:ATC");
                _center = info.GetBoolean("SVEACJ:C");
                _maxCycles = info.GetInt32("SVEACJ:MC");
                _maxDuration = info.GetDouble("SVEACJ:MD");
                if (DataModelVersion.MinorVersion >= 251)
                {
                    _o = (info.GetValue("SVEACJ:O", typeof(Vector3?)) as Vector3?);
                }
            }
            else
            {
                if (GetType() == typeof(SpawnVisualEffectAtCarrierJob))
                {
                    _effectModel = info.GetString("_EffectModel");
                    _attachToCarrier = info.GetBoolean("_AttachToCarrier");
                    _center = info.GetBoolean("_Center");
                    _maxCycles = info.GetInt32("_MaxCycles");
                    _maxDuration = info.GetDouble("_MaxDuration");
                    return;
                }
                _effectModel = info.GetString("SpawnVisualEffectAtCarrierJob+_EffectModel");
                _attachToCarrier = info.GetBoolean("SpawnVisualEffectAtCarrierJob+_AttachToCarrier");
                _center = info.GetBoolean("SpawnVisualEffectAtCarrierJob+_Center");
                _maxCycles = info.GetInt32("SpawnVisualEffectAtCarrierJob+_MaxCycles");
                _maxDuration = info.GetDouble("SpawnVisualEffectAtCarrierJob+_MaxDuration");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SVEACJ:EM", _effectModel);
            info.AddValue("SVEACJ:ATC", _attachToCarrier);
            info.AddValue("SVEACJ:C", _center);
            info.AddValue("SVEACJ:MC", _maxCycles);
            info.AddValue("SVEACJ:MD", _maxDuration);
            info.AddValue("SVEACJ:O", _o);
        }

        private string _effectModel;

        private bool _attachToCarrier;

        private bool _center = true;

        private int _maxCycles = 1;

        private double _maxDuration = double.MaxValue;

        private Vector3? _o;
    }
}
