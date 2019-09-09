using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("A3")]
    [Serializable]
    public class Answer : StoryElement
    {
        protected Answer(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _isDefault = info.GetBoolean("A3:ID");
                if (DataModelVersion.MajorVersion >= 14)
                {
                    _d = info.GetBoolean("A3:D");
                    if (DataModelVersion.MajorVersion >= 15)
                    {
                        _dc = (info.GetValue("A3:DC", typeof(Condition)) as Condition);
                        return;
                    }
                }
            }
            else
            {
                if (GetType() == typeof(Answer))
                {
                    _isDefault = info.GetBoolean("_IsDefault");
                    return;
                }
                _isDefault = info.GetBoolean("Answer+_IsDefault");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("A3:ID", _isDefault);
            info.AddValue("A3:D", _d);
            info.AddValue("A3:DC", _dc);
        }

        private bool _isDefault;

        private bool _d;

        private Condition _dc;
    }
}
