using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("Q99")]
    [Serializable]
    public sealed class Q99 : Job
    {
        private Q99(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _an = info.GetString("Q99:AN");
            _o = (eSPGAOP) info.GetValue("Q99:O", typeof(eSPGAOP));
            SerializationHelper.ReadList<Guid>("Q99:S", ref _s, info);
            if (DataModelVersion.MinorVersion >= 439)
            {
                _e = info.GetBoolean("Q99:E");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("Q99:AN", _an);
            info.AddValue("Q99:O", _o);
            SerializationHelper.WriteList<Guid>("Q99:S", _s, info);
            if (DataModelVersion.MinorVersion >= 439)
            {
                info.AddValue("Q99:E", _e);
            }
        }

        private List<Guid> _s = new List<Guid>();

        private string _an;

        private eSPGAOP _o;

        private bool _e;
    }
}
