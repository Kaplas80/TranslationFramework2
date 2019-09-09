using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DEI")]
    [Serializable]
    internal sealed class DelegateInfo : ISerializable
    {
        public DelegateInfo()
        {
        }

        private DelegateInfo(SerializationInfo info, StreamingContext ctx)
        {
            MethodName = info.GetString("DEI:MN");
            Target = info.GetValue("DEI:T", typeof(object));
            DelegateType = SerializationHelper.a("DEI:DT", info);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("DEI:MN", MethodName);
            info.AddValue("DEI:T", Target);
            SerializationHelper.a("DEI:DT", DelegateType, info);
        }

        public string MethodName;

        public object Target;

        public Type DelegateType;
    }
}
