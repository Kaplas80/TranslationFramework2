using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CC")]
    [Serializable]
    public abstract class CompositeCondition : Condition
    {
        protected CompositeCondition(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                SerializationHelper.ReadCollection<Condition>("CC:EC", ref _conditions, info);
                return;
            }
            
            if (GetType() == typeof(CompositeCondition))
            {
                _conditions = (Collection<Condition>)info.GetValue("_ExecutingConditions", typeof(Collection<Condition>));
                return;
            }
            
            _conditions = (Collection<Condition>)info.GetValue("CompositeCondition+_ExecutingConditions", typeof(Collection<Condition>));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            SerializationHelper.WriteCollection<Condition>("CC:EC", _conditions, info);
        }

        private Collection<Condition> _conditions = new Collection<Condition>();
    }
}
