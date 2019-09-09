using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("Q")]
    [Serializable]
    public class Question : StoryElement
    {
        protected Question(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                SerializationHelper.ReadCollection("Q:PA", ref _possibleAnswers, info);
                _questioner = (Actor) info.GetValue("Q:Q", typeof(Actor));
                _answerer = (Actor) info.GetValue("Q:A", typeof(Actor));
                return;
            }

            if (GetType() == typeof(Question))
            {
                _possibleAnswers = (Collection<Answer>) info.GetValue("_PossibleAnswers", typeof(Collection<Answer>));
                _questioner = (Actor) info.GetValue("_Questioner", typeof(Actor));
                _answerer = (Actor) info.GetValue("_Answerer", typeof(Actor));
                return;
            }

            _possibleAnswers = (Collection<Answer>) info.GetValue("Question+_PossibleAnswers", typeof(Collection<Answer>));
            _questioner = (Actor) info.GetValue("Question+_Questioner", typeof(Actor));
            _answerer = (Actor) info.GetValue("Question+_Answerer", typeof(Actor));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            SerializationHelper.WriteCollection("Q:PA", _possibleAnswers, info);
            info.AddValue("Q:Q", _questioner);
            info.AddValue("Q:A", _answerer);
        }

        public Collection<Answer> PossibleAnswers => _possibleAnswers;

        private Actor _questioner;

        private Actor _answerer;

        private Collection<Answer> _possibleAnswers = new Collection<Answer>();
    }
}
