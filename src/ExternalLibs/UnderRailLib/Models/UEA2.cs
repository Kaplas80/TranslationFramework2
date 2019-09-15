using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Aspects;

namespace UnderRailLib.Models
{
    [EncodedTypeName("UEA2")]
    [Serializable]
    public sealed class UEA2 : Aspect<E4>, iINA, ISerializable
    {
        private UEA2(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _blueprint = info.GetString("UEA2:B");
                SerializationHelper.ReadList("UEA2:F", ref _flags, info);
                return;
            }
            _blueprint = info.GetString("_Blueprint");
            _flags = new List<string>();
            SerializationHelper.ReadList("_Flags", ref _flags, info);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("UEA2:B", _blueprint);
            SerializationHelper.WriteList("UEA2:F", _flags, info);
        }

        private string _blueprint;

        private List<string> _flags = new List<string>();
    }
}
