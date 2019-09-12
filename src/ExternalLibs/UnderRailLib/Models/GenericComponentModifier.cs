using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("GCM")]
    [Serializable]
    public sealed class GenericComponentModifier : ISerializable
    {
        private GenericComponentModifier(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _name = info.GetString("GCM:N");
                _baseValue = info.GetInt32("GCM:BV");
                _increase = info.GetSingle("GCM:I");
                return;
            }

            if (GetType() == typeof(GenericComponentModifier))
            {
                _name = info.GetString("_Name");
                _baseValue = info.GetInt32("_BaseValue");
                _increase = info.GetSingle("_Increase");
                return;
            }

            _name = info.GetString("GenericComponentModifier+_Name");
            _baseValue = info.GetInt32("GenericComponentModifier+_BaseValue");
            _increase = info.GetSingle("GenericComponentModifier+_Increase");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("GCM:N", _name);
            info.AddValue("GCM:BV", _baseValue);
            info.AddValue("GCM:I", _increase);
        }

        private string _name;

        private int _baseValue;

        private float _increase;
    }
}
