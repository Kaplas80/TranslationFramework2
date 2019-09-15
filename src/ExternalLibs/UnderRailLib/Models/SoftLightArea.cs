using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SLA")]
    [Serializable]
    public sealed class SoftLightArea : AreaEffect, iNM, iLY
    {
        private SoftLightArea(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _attached = info.GetBoolean("SLA:A");
                _emitter = (SoftLightEmitter)info.GetValue("SLA:E", typeof(SoftLightEmitter));
                _layer = info.GetInt32("SLA:L");
                return;
            }
            if (GetType() == typeof(SoftLightArea))
            {
                _attached = info.GetBoolean("_Attached");
                _emitter = (SoftLightEmitter)info.GetValue("_Emitter", typeof(SoftLightEmitter));
                _layer = info.GetInt32("_Layer");
                return;
            }
            _attached = info.GetBoolean("SoftLightArea+_Attached");
            _emitter = (SoftLightEmitter)info.GetValue("SoftLightArea+_Emitter", typeof(SoftLightEmitter));
            _layer = info.GetInt32("SoftLightArea+_Layer");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SLA:A", _attached);
            info.AddValue("SLA:E", _emitter);
            info.AddValue("SLA:L", _layer);
        }

        private bool _attached;

        private SoftLightEmitter _emitter;

        private int _layer;
    }
}
