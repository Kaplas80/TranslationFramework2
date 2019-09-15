using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SWC")]
    [Serializable]
    public sealed class StandaloneWorkflowContainer : ISerializable
    {
        private StandaloneWorkflowContainer(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _workflow = (WorkflowRoot) info.GetValue("SWC:W", typeof(WorkflowRoot));
                _workflowState = (eCWS) info.GetValue("SWC:WS", typeof(eCWS));
                if (DataModelVersion.MajorVersion >= 5)
                {
                    SerializationHelper.ReadEvent("SWC:WS1", ref _workflowStarted, info);
                    SerializationHelper.ReadEvent("SWC:WS2", ref _workflowStopped, info);
                    SerializationHelper.ReadEvent("SWC:WC", ref _workflowCompleted, info);
                    SerializationHelper.ReadEvent("SWC:WF", ref _workflowFailed, info);
                    SerializationHelper.ReadEvent("SWC:WP", ref _workflowPaused, info);
                    SerializationHelper.ReadEvent("SWC:WR", ref _workflowResumed, info);
                    return;
                }

                _workflowStarted = (EventHandler<System.EventArgs>) info.GetValue("SWC:WS1", typeof(EventHandler<System.EventArgs>));
                _workflowStopped = (EventHandler<System.EventArgs>) info.GetValue("SWC:WS2", typeof(EventHandler<System.EventArgs>));
                _workflowCompleted = (EventHandler<System.EventArgs>) info.GetValue("SWC:WC", typeof(EventHandler<System.EventArgs>));
                _workflowFailed = (EventHandler<System.EventArgs>) info.GetValue("SWC:WF", typeof(EventHandler<System.EventArgs>));
                _workflowPaused = (EventHandler<System.EventArgs>) info.GetValue("SWC:WP", typeof(EventHandler<System.EventArgs>));
                _workflowResumed = (EventHandler<System.EventArgs>) info.GetValue("SWC:WR", typeof(EventHandler<System.EventArgs>));
            }
            else
            {
                if (GetType() == typeof(StandaloneWorkflowContainer))
                {
                    _workflow = (WorkflowRoot) info.GetValue("_Workflow", typeof(WorkflowRoot));
                    _workflowState = (eCWS) info.GetValue("_WorkflowState", typeof(eCWS));
                    _workflowStarted = (EventHandler<System.EventArgs>) info.GetValue("WorkflowStarted", typeof(EventHandler<System.EventArgs>));
                    _workflowStopped = (EventHandler<System.EventArgs>) info.GetValue("WorkflowStopped", typeof(EventHandler<System.EventArgs>));
                    _workflowCompleted = (EventHandler<System.EventArgs>) info.GetValue("WorkflowCompleted",
                        typeof(EventHandler<System.EventArgs>));
                    _workflowFailed = (EventHandler<System.EventArgs>) info.GetValue("WorkflowFailed", typeof(EventHandler<System.EventArgs>));
                    _workflowPaused = (EventHandler<System.EventArgs>) info.GetValue("WorkflowPaused", typeof(EventHandler<System.EventArgs>));
                    _workflowResumed = (EventHandler<System.EventArgs>) info.GetValue("WorkflowResumed", typeof(EventHandler<System.EventArgs>));
                    return;
                }

                _workflow = (WorkflowRoot) info.GetValue("StandaloneWorkflowContainer+_Workflow", typeof(WorkflowRoot));
                _workflowState = (eCWS) info.GetValue("StandaloneWorkflowContainer+_WorkflowState", typeof(eCWS));
                _workflowStarted = (EventHandler<System.EventArgs>) info.GetValue("StandaloneWorkflowContainer+WorkflowStarted",
                    typeof(EventHandler<System.EventArgs>));
                _workflowStopped = (EventHandler<System.EventArgs>) info.GetValue("StandaloneWorkflowContainer+WorkflowStopped",
                    typeof(EventHandler<System.EventArgs>));
                _workflowCompleted = (EventHandler<System.EventArgs>) info.GetValue("StandaloneWorkflowContainer+WorkflowCompleted",
                    typeof(EventHandler<System.EventArgs>));
                _workflowFailed = (EventHandler<System.EventArgs>) info.GetValue("StandaloneWorkflowContainer+WorkflowFailed",
                    typeof(EventHandler<System.EventArgs>));
                _workflowPaused = (EventHandler<System.EventArgs>) info.GetValue("StandaloneWorkflowContainer+WorkflowPaused",
                    typeof(EventHandler<System.EventArgs>));
                _workflowResumed = (EventHandler<System.EventArgs>) info.GetValue("StandaloneWorkflowContainer+WorkflowResumed",
                    typeof(EventHandler<System.EventArgs>));
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("SWC:W", _workflow);
            info.AddValue("SWC:WS", _workflowState);
            SerializationHelper.WriteEvent("SWC:WS1", _workflowStarted, info);
            SerializationHelper.WriteEvent("SWC:WS2", _workflowStopped, info);
            SerializationHelper.WriteEvent("SWC:WC", _workflowCompleted, info);
            SerializationHelper.WriteEvent("SWC:WF", _workflowFailed, info);
            SerializationHelper.WriteEvent("SWC:WP", _workflowPaused, info);
            SerializationHelper.WriteEvent("SWC:WR", _workflowResumed, info);
        }

        private WorkflowRoot _workflow;

        private eCWS _workflowState;

        private EventHandler<System.EventArgs> _workflowStarted;

        private EventHandler<System.EventArgs> _workflowStopped;

        private EventHandler<System.EventArgs> _workflowCompleted;

        private EventHandler<System.EventArgs> _workflowFailed;

        private EventHandler<System.EventArgs> _workflowPaused;

        private EventHandler<System.EventArgs> _workflowResumed;
    }
}
