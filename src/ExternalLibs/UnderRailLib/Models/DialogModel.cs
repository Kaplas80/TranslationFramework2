using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DM")]
    [Serializable]
    public class DialogModel : ISerializable
    {
        protected DialogModel(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _dialogName = info.GetString("DM:DN");
                _dialogDescription = info.GetString("DM:DD");
                _startElement = (ConditionalElement) info.GetValue("DM:SE", typeof(ConditionalElement));
                SerializationHelper.ReadList<string>("DM:LL", ref _localizationLanguages, info);
                SerializationHelper.ReadList<Actor>("DM:A", ref _actors, info);
                _isCancellable = info.GetBoolean("DM:IC");
                if (DataModelVersion.MajorVersion >= 24)
                {
                    _h = info.GetBoolean("DM:P");
                }
                else
                {
                    _h = !_isCancellable;
                }
            }
            else
            {
                if (GetType() == typeof(DialogModel))
                {
                    _dialogName = info.GetString("_DialogName");
                    _dialogDescription = info.GetString("_DialogDescription");
                    _startElement = (ConditionalElement) info.GetValue("_StartElement", typeof(ConditionalElement));
                    _localizationLanguages = (List<string>) info.GetValue("_LocalizationLanguages", typeof(List<string>));
                    _actors = (List<Actor>) info.GetValue("_Actors", typeof(List<Actor>));
                    _isCancellable = info.GetBoolean("_IsCancellable");
                }
                else
                {
                    _dialogName = info.GetString("DialogModel+_DialogName");
                    _dialogDescription = info.GetString("DialogModel+_DialogDescription");
                    _startElement = (ConditionalElement) info.GetValue("DialogModel+_StartElement", typeof(ConditionalElement));
                    _localizationLanguages = (List<string>) info.GetValue("DialogModel+_LocalizationLanguages", typeof(List<string>));
                    _actors = (List<Actor>) info.GetValue("DialogModel+_Actors", typeof(List<Actor>));
                    _isCancellable = info.GetBoolean("DialogModel+_IsCancellable");
                }
            }
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("DM:DN", _dialogName);
            info.AddValue("DM:DD", _dialogDescription);
            info.AddValue("DM:SE", _startElement);
            SerializationHelper.WriteList<string>("DM:LL", _localizationLanguages, info);
            SerializationHelper.WriteList<Actor>("DM:A", _actors, info);
            info.AddValue("DM:IC", _isCancellable);
            info.AddValue("DM:P", _h);
        }

        public List<string> LocalizationLanguages => _localizationLanguages;

        public List<Actor> Actors => _actors;

        public ConditionalElement StartElement => _startElement;

        private bool _isCancellable;

        private string _dialogName;

        private string _dialogDescription;

        private ConditionalElement _startElement;

        private readonly List<string> _localizationLanguages = new List<string>();
        private readonly List<Actor> _actors = new List<Actor>();
        private bool _h;
    }
}
