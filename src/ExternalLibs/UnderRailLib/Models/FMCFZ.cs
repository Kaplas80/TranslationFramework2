using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("FMCFZ")]
    [Serializable]
    public sealed class FMCFZ : Job
    {
        private FMCFZ(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _fi = info.GetString("FMCFZ:FI");
            _gp = info.GetString("FMCFZ:GP");
            _gpise = info.GetString("FMCFZ:GPISE");
            _aoo = info.GetString("FMCFZ:AOO");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("FMCFZ:FI", _fi);
            info.AddValue("FMCFZ:GP", _gp);
            info.AddValue("FMCFZ:GPISE", _gpise);
            info.AddValue("FMCFZ:AOO", _aoo);
        }

        private string _fi;

        private string _gp;

        private string _gpise;

        private string _aoo;
    }
}
