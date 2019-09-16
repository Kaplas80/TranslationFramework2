using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("AI")]
    [Serializable]
    public sealed class AudioInstance : ISerializable
    {
        private AudioInstance(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _sfxId = info.GetString("AI:SI");
                _volume = info.GetSingle("AI:V");
                _isLooped = info.GetBoolean("AI:IL");
                _paused = info.GetBoolean("AI:P");
                _soundType = (SoundType)info.GetValue("AI:ST", typeof(SoundType));
                _key = (ZSK?)info.GetValue("AI:K", typeof(ZSK?));
                return;
            }
            if (GetType() == typeof(AudioInstance))
            {
                _sfxId = info.GetString("_SfxId");
                _volume = info.GetSingle("_Volume");
                _isLooped = info.GetBoolean("_IsLooped");
                _paused = info.GetBoolean("_Paused");
                _soundType = (SoundType)info.GetValue("_SoundType", typeof(SoundType));
                _key = (ZSK?)info.GetValue("_Key", typeof(ZSK?));
                return;
            }
            _sfxId = info.GetString("AudioInstance+_SfxId");
            _volume = info.GetSingle("AudioInstance+_Volume");
            _isLooped = info.GetBoolean("AudioInstance+_IsLooped");
            _paused = info.GetBoolean("AudioInstance+_Paused");
            _soundType = (SoundType)info.GetValue("AudioInstance+_SoundType", typeof(SoundType));
            _key = (ZSK?)info.GetValue("AudioInstance+_Key", typeof(ZSK?));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("AI:SI", _sfxId);
            info.AddValue("AI:V", _volume);
            info.AddValue("AI:IL", _isLooped);
            info.AddValue("AI:P", _paused);
            info.AddValue("AI:ST", _soundType);
            info.AddValue("AI:K", _key);
        }

        private string _sfxId;

        private float _volume = 1f;

        private bool _isLooped;

        private bool _paused;

        private SoundType _soundType;

        private ZSK? _key;
    }
}
