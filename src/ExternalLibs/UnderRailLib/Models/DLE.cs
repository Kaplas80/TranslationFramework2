using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DLE")]
    [Serializable]
    public abstract class DLE : ISerializable
    {
        protected DLE(SerializationInfo info, StreamingContext ctx)
        {
            _c = info.GetString("DLE:C");
            _ct = (info.GetValue("DLE:CT", typeof(TimeSpan?)) as TimeSpan?);
            _dp = (info.GetValue("DLE:DP", typeof(TimeSpan?)) as TimeSpan?);
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("DLE:C", _c);
            info.AddValue("DLE:CT", _ct);
            info.AddValue("DLE:DP", _dp);
        }

        private string _c;

        private TimeSpan? _ct;

        private TimeSpan? _dp;
    }
}