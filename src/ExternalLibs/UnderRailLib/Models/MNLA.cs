using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MNLA")]
    [Serializable]
    public sealed class MNLA : BaseAction
    {
        private MNLA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _m = info.GetSingle("MNLA:M");
            _nn = info.GetString("MNLA:NN");
            if (DataModelVersion.MinorVersion >= 103)
            {
                _g = info.GetBoolean("MNLA:G");
                _s = info.GetBoolean("MNLA:S");
                return;
            }

            _g = !info.GetBoolean("MNLA:L");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MNLA:M", _m);
            info.AddValue("MNLA:NN", _nn);
            if (DataModelVersion.MinorVersion >= 103)
            {
                info.AddValue("MNLA:G", _g);
                info.AddValue("MNLA:S", _s);
            }
        }

        private float _m;

        private string _nn;

        private bool _g;

        private bool _s;
    }
}
