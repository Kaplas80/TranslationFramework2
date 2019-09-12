using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("G12")]
    [Serializable]
    public sealed class G12 : ItemGeneratorBase, ISerializable
    {
        private G12(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _m = info.GetString("G12:M");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("G12:M", _m);
        }

        private string _m;
    }
}
