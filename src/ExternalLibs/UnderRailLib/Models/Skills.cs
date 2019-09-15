using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.EventArgs;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("S3")]
    [Serializable]
    public class Skills : iCHP, c4w, ISerializable
    {
        protected Skills(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _character = (C1) info.GetValue("S3:C", typeof(C1));
                SerializationHelper.ReadList("S3:S", ref _skills, info);
                if (DataModelVersion.MajorVersion >= 5)
                {
                    SerializationHelper.ReadEvent("S3:SA", ref _sa, info);
                    SerializationHelper.ReadEvent("S3:SVC", ref _skillValueChanged, info);
                    return;
                }

                _skillValueChanged = (EventHandler<eaPCEA>) info.GetValue("S3:SVC", typeof(EventHandler<eaPCEA>));
            }
            else
            {
                if (GetType() == typeof(Skills))
                {
                    _character = (C1) info.GetValue("_Character", typeof(C1));
                    _skills = (List<Skill>) info.GetValue("_Skills", typeof(List<Skill>));
                    _skillValueChanged = (EventHandler<eaPCEA>) info.GetValue("SkillValueChanged", typeof(EventHandler<eaPCEA>));
                    return;
                }

                _character = (C1) info.GetValue("Skills+_Character", typeof(C1));
                _skills = (List<Skill>) info.GetValue("Skills+_Skills", typeof(List<Skill>));
                _skillValueChanged = (EventHandler<eaPCEA>) info.GetValue("Skills+SkillValueChanged", typeof(EventHandler<eaPCEA>));
            }
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("S3:C", _character);
            SerializationHelper.WriteList("S3:S", _skills, info);
            SerializationHelper.WriteEvent("S3:SA", _sa, info);
            SerializationHelper.WriteEvent("S3:SVC", _skillValueChanged, info);
        }

        private C1 _character;

        internal List<Skill> _skills = new List<Skill>();

        private EventHandler<System.EventArgs> _sa;

        private EventHandler<eaPCEA> _skillValueChanged;
    }
}
