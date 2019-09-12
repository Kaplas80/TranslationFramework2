using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PCJ")]
    [Serializable]
    public sealed class PlayCutsceneJob : Job
    {
        private PlayCutsceneJob(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _cutsceneWorkflowPath = info.GetString("PCJ:CWP");
                return;
            }
            
            if (GetType() == typeof(PlayCutsceneJob))
            {
                _cutsceneWorkflowPath = info.GetString("_CutsceneWorkflowPath");
                return;
            }
            _cutsceneWorkflowPath = info.GetString("PlayCutsceneJob+_CutsceneWorkflowPath");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PCJ:CWP", _cutsceneWorkflowPath);
        }

        private string _cutsceneWorkflowPath;
    }
}
