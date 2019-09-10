using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SQA")]
    [Serializable]
    public sealed class StartQuestAction : BaseAction
    {
        private StartQuestAction(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _questName = info.GetString("SQA:QN");
                if (DataModelVersion.MinorVersion >= 16)
                {
                    _pe = info.GetBoolean("SQA:PE");
                    return;
                }

                _pe = true;
            }
            else
            {
                if (GetType() == typeof(StartQuestAction))
                {
                    _questName = info.GetString("_QuestName");
                    return;
                }

                _questName = info.GetString("StartQuestAction+_QuestName");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SQA:QN", _questName);
            if (DataModelVersion.MinorVersion >= 16)
            {
                info.AddValue("SQA:PE", _pe);
            }
        }

        private bool _pe = true;

        private string _questName;
    }
}
