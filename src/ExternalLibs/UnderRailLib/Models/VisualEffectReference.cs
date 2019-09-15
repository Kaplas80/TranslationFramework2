using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("VER")]
    [Serializable]
    public sealed class VisualEffectReference : ISerializable
    {
        private VisualEffectReference(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _name = info.GetString("VER:N");
                _context = info.GetValue("VER:C", typeof(object));
                return;
            }

            if (GetType() == typeof(VisualEffectReference))
            {
                _name = info.GetString("Name");
                _context = info.GetValue("Context", typeof(object));
                return;
            }

            _name = info.GetString("VisualEffectReference+Name");
            _context = info.GetValue("VisualEffectReference+Context", typeof(object));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("VER:N", _name);
            info.AddValue("VER:C", _context);
        }

        public string _name;

        public object _context;
    }
}
