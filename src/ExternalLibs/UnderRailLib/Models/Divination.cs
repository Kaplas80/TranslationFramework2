using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Data;

namespace UnderRailLib.Models
{
    [EncodedTypeName("D")]
    [Serializable]
    public class Divination : AreaEffect, iNM, iLY
    {
        protected Divination(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _attached = info.GetBoolean("D:A");
                _observerReference = (TypedObjectReference<Observer>)info.GetValue("D:OR", typeof(TypedObjectReference<Observer>));
                _layer = info.GetInt32("D:L");
                return;
            }
            if (GetType() == typeof(Divination))
            {
                _attached = info.GetBoolean("_Attached");
                _observerReference = (TypedObjectReference<Observer>)info.GetValue("_ObserverReference", typeof(TypedObjectReference<Observer>));
                _layer = info.GetInt32("_Layer");
                return;
            }
            _attached = info.GetBoolean("Divination+_Attached");
            _observerReference = (TypedObjectReference<Observer>)info.GetValue("Divination+_ObserverReference", typeof(TypedObjectReference<Observer>));
            _layer = info.GetInt32("Divination+_Layer");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("D:A", _attached);
            info.AddValue("D:OR", _observerReference);
            info.AddValue("D:L", _layer);
        }

        private bool _attached;

        private TypedObjectReference<Observer> _observerReference;

        private int _layer;
    }
}
