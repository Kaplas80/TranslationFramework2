using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MSFE2")]
    [Serializable]
    public sealed class MSFE2 : ModifyStatFlatEffect, iSKFX
    {
        private MSFE2(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _s = (info.GetValue("MSFE2:S", typeof(List<string>)) as List<string>);
            _n = info.GetString("MSFE2:N");
            if (_n == "Psionic skills" && _s != null && !_s.Contains("Temporal Manipulation"))
            {
                _s.Add("Temporal Manipulation");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("MSFE2:S", _s);
            info.AddValue("MSFE2:N", _n);
        }

        private List<string> _s;

        private string _n;
    }
}
