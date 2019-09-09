using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SE")]
    [Serializable]
    public class StoryElement : ConditionalElement
    {
        protected StoryElement(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                SerializationHelper.ReadCollection("SE:RA", ref _resultingActions, info);
                SerializationHelper.ReadDictionary("SE:LT", ref _localizedTexts, info);
                return;
            }

            if (GetType() == typeof(StoryElement))
            {
                _resultingActions = (Collection<BaseAction>) info.GetValue("_ResultingActions", typeof(Collection<BaseAction>));
                _localizedTexts = (Dictionary<string, string>) info.GetValue("_LocalizedTexts",
                    typeof(Dictionary<string, string>));
                return;
            }

            _resultingActions = (Collection<BaseAction>) info.GetValue("StoryElement+_ResultingActions", typeof(Collection<BaseAction>));
            _localizedTexts = (Dictionary<string, string>) info.GetValue("StoryElement+_LocalizedTexts",
                typeof(Dictionary<string, string>));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            SerializationHelper.WriteCollection("SE:RA", _resultingActions, info);
            SerializationHelper.WriteDictionary("SE:LT", _localizedTexts, info);
        }

        public Dictionary<string, string> LocalizedTexts => _localizedTexts;

        private readonly Collection<BaseAction> _resultingActions = new Collection<BaseAction>();

        private readonly Dictionary<string, string> _localizedTexts = new Dictionary<string, string>();
    }
}
