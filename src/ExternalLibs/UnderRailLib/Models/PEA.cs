using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Ouroboros.Common.Aspects;

namespace UnderRailLib.Models
{
    [EncodedTypeName("PEA")]
    [Serializable]
    public sealed class PEA : Aspect<LE2>
    {
        private PEA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                SerializationHelper.ReadList("PEA:O", ref _o, info);
                SerializationHelper.ReadList("PEA:OF", ref _of, info);
                SerializationHelper.ReadList("PEA:P", ref _p, info);
                SerializationHelper.ReadList("PEA:PF", ref _pf, info);
                _pbm = (eFBCM)info.GetValue("PEA:PBM", typeof(eFBCM));
                _spe = info.GetBoolean("PEA:SPE");
                return;
            }
            SerializationHelper.ReadList("PEA_O", ref _o, info);
            SerializationHelper.ReadList("PEA_OF", ref _of, info);
            SerializationHelper.ReadList("PEA_P", ref _p, info);
            SerializationHelper.ReadList("PEA_PF", ref _pf, info);
            _pbm = (eFBCM)info.GetValue("PEA_BM", typeof(eFBCM));
            _spe = info.GetBoolean("PEA_SPE");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            SerializationHelper.WriteList("PEA:O", _o, info);
            SerializationHelper.WriteList("PEA:OF", _of, info);
            SerializationHelper.WriteList("PEA:P", _p, info);
            SerializationHelper.WriteList("PEA:PF", _pf, info);
            info.AddValue("PEA:PBM", _pbm);
            info.AddValue("PEA:SPE", _spe);
        }

        private List<string> _o = new List<string>();

        private List<string> _of = new List<string>();

        private List<string> _p = new List<string>();

        private List<string> _pf = new List<string>();

        private eFBCM _pbm = eFBCM.b;

        private bool _spe = true;
    }
}
