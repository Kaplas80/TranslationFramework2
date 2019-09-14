using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PLSJ")]
    [Serializable]
    public sealed class PLSJ : LSJ
    {
        private PLSJ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion >= 15)
            {
                _scic = info.GetBoolean("PLSJ:SCIC");
                _swfp = info.GetBoolean("PLSJ:SWFP");
                if (DataModelVersion.MinorVersion >= 41)
                {
                    _siup = info.GetBoolean("PLSJ:SIUP");
                }
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("PLSJ:SCIC", _scic);
            info.AddValue("PLSJ:SWFP", _swfp);
            if (DataModelVersion.MinorVersion >= 41)
            {
                info.AddValue("PLSJ:SIUP", _siup);
            }
        }

        private bool _scic;

        private bool _swfp;

        private bool _siup;
    }
}
