using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Aspects;

namespace UnderRailLib.Models
{
    [EncodedTypeName("TASEA")]
    [Serializable]
    public class TASEA : Aspect<LE2>, iINA, dfi
    {
        protected TASEA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion == 0)
            {
                SerializationHelper.ReadDictionary("_AudioInstances", ref _audioInstances, info);
                return;
            }
            SerializationHelper.ReadDictionary("TASEA:AI", ref _audioInstances, info);
            if (DataModelVersion.MinorVersion >= 369)
            {
                _sn = info.GetBoolean("TASEA:SN");
                return;
            }
            _sn = true;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            SerializationHelper.WriteDictionary("TASEA:AI", _audioInstances, info);
            info.AddValue("TASEA:SN", _sn);
        }

        private Dictionary<string, AudioInstance> _audioInstances = new Dictionary<string, AudioInstance>();

        private bool _sn = true;
    }
}
