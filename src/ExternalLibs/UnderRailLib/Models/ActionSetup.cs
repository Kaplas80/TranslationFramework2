using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("AS")]
    [Serializable]
    public sealed class ActionSetup : ISerializable
    {
        private ActionSetup(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _action = (iACT)info.GetValue("AS:A", typeof(iACT));
                _executionType = (ActionExecutionType)info.GetValue("AS:ET", typeof(ActionExecutionType));
                _cancelled = info.GetBoolean("AS:C");
                return;
            }
            if (GetType() == typeof(ActionSetup))
            {
                _action = (iACT)info.GetValue("Action", typeof(iACT));
                _executionType = (ActionExecutionType)info.GetValue("ExecutionType", typeof(ActionExecutionType));
                _cancelled = info.GetBoolean("Cancelled");
                return;
            }
            _action = (iACT)info.GetValue("ActionSetup+Action", typeof(iACT));
            _executionType = (ActionExecutionType)info.GetValue("ActionSetup+ExecutionType", typeof(ActionExecutionType));
            _cancelled = info.GetBoolean("ActionSetup+Cancelled");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("AS:A", _action);
            info.AddValue("AS:ET", _executionType);
            info.AddValue("AS:C", _cancelled);
        }

        public iACT _action;

        public ActionExecutionType _executionType;

        public bool _cancelled;
    }
}
