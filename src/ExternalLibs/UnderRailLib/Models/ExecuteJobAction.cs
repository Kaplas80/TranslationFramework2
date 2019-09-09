using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("EJA")]
    [Serializable]
    public sealed class ExecuteJobAction : BaseAction, ISerializable
    {
        private ExecuteJobAction(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _job = (Job)info.GetValue("EJA:J", typeof(Job));
                return;
            }
            if (GetType() == typeof(ExecuteJobAction))
            {
                _job = (Job)info.GetValue("_Job", typeof(Job));
                return;
            }
            _job = (Job)info.GetValue("ExecuteJobAction+_Job", typeof(Job));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("EJA:J", _job);
        }

        private Job _job;
    }
}
