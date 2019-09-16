using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Playfield.References;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PCAC")]
    [Serializable]
    public sealed class PCAC : ISerializable
    {
        private PCAC(SerializationInfo info, StreamingContext ctx)
        {
            _a = (info.GetValue("PCAC:A", typeof(ActionReference<iPRCHA>)) as ActionReference<iPRCHA>);
            _e = (eAEXT)info.GetValue("PCAC:E", typeof(eAEXT));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("PCAC:A", _a);
            info.AddValue("PCAC:E", _e);
        }

        public PCAC(ActionReference<iPRCHA> a, eAEXT e)
        {
            _a = a;
            _e = e;
        }

        public ActionReference<iPRCHA> _a;

        public eAEXT _e;
    }
}
