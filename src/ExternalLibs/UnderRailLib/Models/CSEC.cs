using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Comm;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CSEC")]
    [Serializable]
    public sealed class CSEC : Condition
    {
        private CSEC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _event = (CombatSpeakEvent)info.GetValue("CSEC:E", typeof(CombatSpeakEvent));
            _p = info.GetInt32("CSEC:P");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CSEC:E", _event);
            info.AddValue("CSEC:P", _p);
        }

        private int _p;

        private CombatSpeakEvent _event;
    }
}
