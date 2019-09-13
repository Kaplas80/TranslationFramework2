using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MSRTJ")]
    [Serializable]
    public sealed class MoveShootingRangeTargetJob : Job
    {
        private MoveShootingRangeTargetJob(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _distance = info.GetInt32("MSRTJ:D");
                return;
            }

            if (GetType() == typeof(MoveShootingRangeTargetJob))
            {
                _distance = info.GetInt32("_Distance");
                return;
            }

            _distance = info.GetInt32("MoveShootingRangeTargetJob+_Distance");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MSRTJ:D", _distance);
        }

        private int _distance;
    }
}