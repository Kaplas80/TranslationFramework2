using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.TimelapseVertigo.Rules.Characters;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PGC")]
    [Serializable]
    public sealed class PGC : Condition
    {
        private PGC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _gender = (Gender)info.GetValue("PGC:G", typeof(Gender));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PGC:G", _gender);
        }

        private Gender _gender;
    }
}
