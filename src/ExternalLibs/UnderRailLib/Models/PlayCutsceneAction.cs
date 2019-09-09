using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PCA1")]
    [Serializable]
    public sealed class PlayCutsceneAction : BaseAction
    {
        private PlayCutsceneAction(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _workflowPath = info.GetString("PCA1:WP");
                return;
            }
            
            if (GetType() == typeof(PlayCutsceneAction))
            {
                _workflowPath = info.GetString("_WorkflowPath");
                return;
            }
            _workflowPath = info.GetString("PlayCutsceneAction+_WorkflowPath");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PCA1:WP", _workflowPath);
        }

        private string _workflowPath;
    }
}
