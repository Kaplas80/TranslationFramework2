using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CE")]
    [Serializable]
    public abstract class ConditionalElement : ISerializable
    {
        protected ConditionalElement(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _causingCondition = (Condition) info.GetValue("CE:CC", typeof(Condition));
                SerializationHelper.ReadCollection<ConditionalElement>("CE:PNS", ref _possibleNextSteps, info);
                Name = info.GetString("CE:N");
                return;
            }

            if (GetType() == typeof(ConditionalElement))
            {
                _causingCondition = (Condition) info.GetValue("_CausingCondition", typeof(Condition));
                _possibleNextSteps = (Collection<ConditionalElement>) info.GetValue("_PossibleNextSteps",
                    typeof(Collection<ConditionalElement>));
                Name = info.GetString("_Name");
                return;
            }

            _causingCondition = (Condition) info.GetValue("ConditionalElement+_CausingCondition", typeof(Condition));
            _possibleNextSteps = (Collection<ConditionalElement>) info.GetValue("ConditionalElement+_PossibleNextSteps",
                typeof(Collection<ConditionalElement>));
            Name = info.GetString("ConditionalElement+_Name");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("CE:CC", _causingCondition);
            SerializationHelper.WriteCollection<ConditionalElement>("CE:PNS", _possibleNextSteps, info);
            info.AddValue("CE:N", Name);
        }

        public string Name { get; }

        public Collection<ConditionalElement> PossibleNextSteps => _possibleNextSteps;

        private Condition _causingCondition;

        private Collection<ConditionalElement> _possibleNextSteps = new Collection<ConditionalElement>();
    }
}
