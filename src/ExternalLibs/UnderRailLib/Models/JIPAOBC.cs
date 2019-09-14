using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("JIPAOBC")]
    [Serializable]
    public sealed class JIPAOBC : Condition
    {
        private JIPAOBC(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = info.GetBoolean("JIPAOBC:I");
            _f = info.GetString("JIPAOBC:F");
            _c = info.GetString("JIPAOBC:C");
                
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("JIPAOBC:I", _i);
            info.AddValue("JIPAOBC:F", _f);
            info.AddValue("JIPAOBC:C", _c);    
        }

        private bool _i;

        private string _f;

        private string _c;
    }
}