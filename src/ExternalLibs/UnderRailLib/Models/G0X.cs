using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("G0X")]
    [Serializable]
    public sealed class G0X : ItemGeneratorBase, ISerializable
    {
        private G0X(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _p = info.GetString("G0X:P");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("G0X:P", _p);
        }

        private string _p;
    }
}
