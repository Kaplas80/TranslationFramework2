using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("C22")]
    [Serializable]
    public sealed class C22 : MSPE2, iMS
    {
        private C22(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion >= 165)
            {
                _f = (eWC)info.GetValue("C22:F", typeof(eWC));
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("C22:F", _f);
        }
 
        private eWC _f;
    }
}
