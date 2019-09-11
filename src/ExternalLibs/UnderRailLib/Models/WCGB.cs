using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("WCGB")]
    [Serializable]
    public abstract class WCGB : ItemGeneratorBase, ISerializable
    {
        protected WCGB(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion >= 136)
            {
                _a = info.GetInt32("WCGB:A");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("WCGB:A", _a);
        }

        private int _a;
    }
}
