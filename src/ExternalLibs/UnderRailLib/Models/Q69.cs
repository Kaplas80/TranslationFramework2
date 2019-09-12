using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("Q69")]
    [Serializable]
    public sealed class Q69 : Job
    {
        private Q69(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _an = info.GetString("Q69:AN");
            _o = (SplitGateOperation) info.GetValue("Q69:O", typeof(SplitGateOperation));
            SerializationHelper.ReadList("Q69:S", ref _s, info);
            if (DataModelVersion.MinorVersion >= 439)
            {
                _e = info.GetBoolean("Q69:E");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("Q69:AN", _an);
            info.AddValue("Q69:O", _o);
            SerializationHelper.WriteList("Q69:S", _s, info);
            if (DataModelVersion.MinorVersion >= 439)
            {
                info.AddValue("Q69:E", _e);
            }
        }

        private List<Guid> _s = new List<Guid>();

        private string _an;

        private SplitGateOperation _o;

        private bool _e;
    }
}
