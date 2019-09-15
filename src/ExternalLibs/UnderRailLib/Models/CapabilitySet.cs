using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CS4")]
    [Serializable]
    public sealed class CapabilitySet : iNM, ISerializable
    {
        private CapabilitySet(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _name = info.GetString("CS4:N");
                _sortedList = (SortedList<CapabilityReference, CapabilityReference>)info.GetValue("CS4:SL", typeof(SortedList<CapabilityReference, CapabilityReference>));
                return;
            }
            if (GetType() == typeof(CapabilitySet))
            {
                _name = info.GetString("_Name");
                _sortedList = (SortedList<CapabilityReference, CapabilityReference>)info.GetValue("_SortedList", typeof(SortedList<CapabilityReference, CapabilityReference>));
                return;
            }
            _name = info.GetString("CapabilitySet+_Name");
            _sortedList = (SortedList<CapabilityReference, CapabilityReference>)info.GetValue("CapabilitySet+_SortedList", typeof(SortedList<CapabilityReference, CapabilityReference>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("CS4:N", _name);
            info.AddValue("CS4:SL", _sortedList);
        }

        private string _name;

        private SortedList<CapabilityReference, CapabilityReference> _sortedList;
    }
}
