using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.TimelapseVertigo.Rules.Characters;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CI1")]
    [Serializable]
    public sealed class CharacterInfo : ISerializable
    {
        private CharacterInfo(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _name = info.GetString("CI1:N");
                _factionId = info.GetString("CI1:FI");
                _passive = info.GetBoolean("CI1:P");
                _uid = (Guid)info.GetValue("CI1:U", typeof(Guid));
                if (DataModelVersion.MinorVersion >= 22)
                {
                    _t = (eCT3)info.GetValue("CT1:T", typeof(eCT3));
                }
                if (DataModelVersion.MinorVersion >= 152)
                {
                    _gender = (Gender)info.GetValue("CT1:G", typeof(Gender));
                }
                if (DataModelVersion.MinorVersion >= 171)
                {
                    _a = (eCAT)info.GetValue("CT1:A", typeof(eCAT));
                }
                if (DataModelVersion.MinorVersion >= 206)
                {
                    SerializationHelper.ReadList("CT1:F", ref _f, info);
                }
                else
                {
                    _f = new List<string>();
                }
                if (DataModelVersion.MinorVersion >= 366)
                {
                    _d = info.GetBoolean("CI1:D");
                }
                if (DataModelVersion.MinorVersion >= 396)
                {
                    SerializationHelper.ReadDictionary("CT1:S", ref _s, info);
                    return;
                }
                _s = new Dictionary<string, int>();
            }
            else
            {
                if (GetType() == typeof(CharacterInfo))
                {
                    _name = info.GetString("Name");
                    _factionId = info.GetString("FactionId");
                    _passive = info.GetBoolean("Passive");
                    _uid = (Guid)info.GetValue("Uid", typeof(Guid));
                    return;
                }
                _name = info.GetString("CharacterInfo+Name");
                _factionId = info.GetString("CharacterInfo+FactionId");
                _passive = info.GetBoolean("CharacterInfo+Passive");
                _uid = (Guid)info.GetValue("CharacterInfo+Uid", typeof(Guid));
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("CI1:N", _name);
            info.AddValue("CI1:FI", _factionId);
            info.AddValue("CI1:P", _passive);
            info.AddValue("CI1:U", _uid);
            info.AddValue("CT1:T", _t);
            info.AddValue("CT1:G", _gender);
            info.AddValue("CT1:A", _a);
            info.AddValue("CI1:D", _d);
            SerializationHelper.WriteList("CT1:F", _f, info);
            SerializationHelper.WriteDictionary("CT1:S", _s, info);
        }
        
        public string _name;

        public string _factionId;

        public bool _passive;

        public Guid _uid;

        public eCT3 _t;

        public Gender _gender;

        public eCAT _a;

        public List<string> _f = new List<string>();

        public Dictionary<string, int> _s = new Dictionary<string, int>();

        public bool _d;
    }
}
