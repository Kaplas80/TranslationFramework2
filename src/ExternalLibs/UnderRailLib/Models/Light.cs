using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Ouroboros.Playfield.Lightning;

namespace UnderRailLib.Models
{
    [EncodedTypeName("L1")]
    [Serializable]
    public struct Light : ISerializable
    {
        private Light(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion >= 6)
            {
                _color = (Color)info.GetValue("C", typeof(Color));
                _intensity = info.GetSingle("I");
                _type = (LightType)info.GetValue("T", typeof(LightType));
                return;
            }
            _color = (Color)info.GetValue("_Color", typeof(Color));
            _intensity = info.GetSingle("_Intensity");
            _type = (LightType)info.GetValue("_Type", typeof(LightType));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("C", _color);
            info.AddValue("I", _intensity);
            info.AddValue("T", _type);
        }

        private Color _color;

        private float _intensity;

        private LightType _type;
    }
}
