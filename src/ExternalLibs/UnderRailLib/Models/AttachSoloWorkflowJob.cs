using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ASWJ")]
    [Serializable]
    public sealed class AttachSoloWorkflowJob : Job
    {
        private AttachSoloWorkflowJob(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _workflowPath = info.GetString("ASWJ:WP");
                _queueStartAfterAttaching = info.GetBoolean("ASWJ:QSAA");
                return;
            }

            if (GetType() == typeof(AttachSoloWorkflowJob))
            {
                _workflowPath = info.GetString("_WorkflowPath");
                _queueStartAfterAttaching = info.GetBoolean("_QueueStartAfterAttaching");
                return;
            }

            _workflowPath = info.GetString("AttachSoloWorkflowJob+_WorkflowPath");
            _queueStartAfterAttaching = info.GetBoolean("AttachSoloWorkflowJob+_QueueStartAfterAttaching");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("ASWJ:WP", _workflowPath);
            info.AddValue("ASWJ:QSAA", _queueStartAfterAttaching);
        }

        private string _workflowPath;

        private bool _queueStartAfterAttaching;
    }
}