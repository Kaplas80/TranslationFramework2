using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("A7")]
    [Serializable]
    public sealed class Auras : ISerializable
    {
        private Auras(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _character = (C1)info.GetValue("A7:C", typeof(C1));
                SerializationHelper.ReadDictionary("A7:AA", ref _activeAuras, info);
                SerializationHelper.ReadList("A7:USE", ref _unappliedStatusEffect, info);
                _updateCache = info.GetDouble("A7:UC");
                return;
            }
            if (GetType() == typeof(Auras))
            {
                _character = (C1)info.GetValue("_Character", typeof(C1));
                _activeAuras = (Dictionary<CapabilityReference, AuraContext>)info.GetValue("_ActiveAuras", typeof(Dictionary<CapabilityReference, AuraContext>));
                _unappliedStatusEffect = (List<SE1>)info.GetValue("_UnappliedStatusEffect", typeof(List<SE1>));
                _updateCache = info.GetDouble("_UpdateCache");
                return;
            }
            _character = (C1)info.GetValue("Auras+_Character", typeof(C1));
            _activeAuras = (Dictionary<CapabilityReference, AuraContext>)info.GetValue("Auras+_ActiveAuras", typeof(Dictionary<CapabilityReference, AuraContext>));
            _unappliedStatusEffect = (List<SE1>)info.GetValue("Auras+_UnappliedStatusEffect", typeof(List<SE1>));
            _updateCache = info.GetDouble("Auras+_UpdateCache");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("A7:C", _character);
            SerializationHelper.WriteDictionary("A7:AA", _activeAuras, info);
            SerializationHelper.WriteList("A7:USE", _unappliedStatusEffect, info);
            info.AddValue("A7:UC", _updateCache);
        }

        private C1 _character;

        private Dictionary<CapabilityReference, AuraContext> _activeAuras = new Dictionary<CapabilityReference, AuraContext>();

        private List<SE1> _unappliedStatusEffect = new List<SE1>();

        private double _updateCache;
    }
}
