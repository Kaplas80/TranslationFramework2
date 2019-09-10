using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("LR")]
    [Serializable]
    public class LocaleReference : ISerializable
    {
        public LocaleReference()
        {

        }

        protected LocaleReference(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _localeId = info.GetString("LR:LI");
                _entranceId = info.GetString("LR:EI");
                return;
            }

            if (GetType() == typeof(LocaleReference))
            {
                _localeId = info.GetString("_LocaleId");
                _entranceId = info.GetString("_EntranceId");
                return;
            }

            _localeId = info.GetString("LocaleReference+_LocaleId");
            _entranceId = info.GetString("LocaleReference+_EntranceId");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("LR:LI", _localeId);
            info.AddValue("LR:EI", _entranceId);
        }

        private string _localeId;

        private string _entranceId;
    }
}
