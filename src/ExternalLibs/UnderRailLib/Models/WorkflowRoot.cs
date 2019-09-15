using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("WR")]
    [Serializable]
    public class WorkflowRoot : SequentialCompositeActivity, ISerializable
    {
        protected WorkflowRoot(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                SerializationHelper.ReadDictionary("WR:C", ref _contexts, info);
                return;
            }
            if (GetType() == typeof(WorkflowRoot))
            {
                _contexts = (Dictionary<string, iPC>)info.GetValue("_Contexts", typeof(Dictionary<string, iPC>));
                return;
            }
            _contexts = (Dictionary<string, iPC>)info.GetValue("WorkflowRoot+_Contexts", typeof(Dictionary<string, iPC>));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            SerializationHelper.WriteDictionary("WR:C", _contexts, info);
        }

        private Dictionary<string, iPC> _contexts = new Dictionary<string, iPC>();
    }
}
