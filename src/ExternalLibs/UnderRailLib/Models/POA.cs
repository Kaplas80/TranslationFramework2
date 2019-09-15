using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Aspects;

namespace UnderRailLib.Models
{
    [EncodedTypeName("POA")]
    [Serializable]
    public sealed class POA : Aspect<LE2>, iINA
    {
        private POA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion >= 69)
            {
                _p = (info.GetValue("POA:P", typeof(bool?)) as bool?);
            }
            else
            {
                _p = info.GetBoolean("POA:P");
            }
            SerializationHelper.ReadList("POA:PS", ref _ps, info);
            if (DataModelVersion.MinorVersion >= 37)
            {
                SerializationHelper.ReadList("POA:PONJ", ref _ponj, info);
                SerializationHelper.ReadList("POA:POFJ", ref _pofj, info);
                return;
            }
            _ponj = new List<Job>();
            _pofj = new List<Job>();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("POA:P", _p);
            SerializationHelper.WriteList("POA:PS", _ps, info);
            SerializationHelper.WriteList("POA:PONJ", _ponj, info);
            SerializationHelper.WriteList("POA:POFJ", _pofj, info);
        }

        private bool? _p;

        private List<string> _ps = new List<string>();

        private List<Job> _ponj = new List<Job>();

        private List<Job> _pofj = new List<Job>();
    }
}
