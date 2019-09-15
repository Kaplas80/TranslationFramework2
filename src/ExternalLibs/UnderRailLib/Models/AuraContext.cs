using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("AC")]
    [Serializable]
    public class AuraContext : ISerializable
    {
        protected AuraContext(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                SerializationHelper.ReadList("AC:SE", ref _statusEffects, info);
                _invocationSet = (iIVS)info.GetValue("AC:IS", typeof(iIVS));
                return;
            }
            if (GetType() == typeof(AuraContext))
            {
                _statusEffects = (List<SE1>)info.GetValue("_StatusEffects", typeof(List<SE1>));
                _invocationSet = (iIVS)info.GetValue("_InvocationSet", typeof(iIVS));
                return;
            }
            _statusEffects = (List<SE1>)info.GetValue("AuraContext+_StatusEffects", typeof(List<SE1>));
            _invocationSet = (iIVS)info.GetValue("AuraContext+_InvocationSet", typeof(iIVS));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            SerializationHelper.WriteList("AC:SE", _statusEffects, info);
            info.AddValue("AC:IS", _invocationSet);
        }

        private List<SE1> _statusEffects;

        private iIVS _invocationSet;
    }
}
