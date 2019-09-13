using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SSRTSJ")]
    [Serializable]
    public sealed class SetShootingRangeTargetStateJob : Job
    {
        private SetShootingRangeTargetStateJob(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _frameX = info.GetInt32("SSRTSJ:FX");
                return;
            }
            if (GetType() == typeof(SetShootingRangeTargetStateJob))
            {
                _frameX = info.GetInt32("_FrameX");
                return;
            }
            _frameX = info.GetInt32("SetShootingRangeTargetStateJob+_FrameX");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SSRTSJ:FX", _frameX);
        }

        private int _frameX;
    }
}
