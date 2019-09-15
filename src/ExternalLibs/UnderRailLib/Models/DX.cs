using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Aspects;
using UnderRailLib.Ouroboros.Playfield.References;

namespace UnderRailLib.Models
{
    [EncodedTypeName("DX")]
    [Serializable]
    public sealed class DX : Aspect<LE2>, iMAIA, iINA
    {
        private DX(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _rg = info.GetString("DX:RG");
            _wc = info.GetInt32("DX:WC");
            _pwi = info.GetInt32("DX:PWI");
            _csi = info.GetInt32("DX:CSI");
            _qs = info.GetInt32("DX:QS");
            _pwc = info.GetDouble("DX:PWC");
            _csc = info.GetDouble("DX:CSC");
            _r = (info.GetValue("DX:R", typeof(Queue<SDR>)) as Queue<SDR>);
            SerializationHelper.ReadList("DX:F", ref _f, info);
            SerializationHelper.ReadDictionary("DX:A", ref _a, info);
            if (DataModelVersion.MinorVersion >= 246)
            {
                SerializationHelper.ReadList("DX:J", ref _j, info);
                return;
            }
            _j = new List<Job>();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("DX:RG", _rg);
            info.AddValue("DX:WC", _wc);
            info.AddValue("DX:PWI", _pwi);
            info.AddValue("DX:CSI", _csi);
            info.AddValue("DX:QS", _qs);
            info.AddValue("DX:PWC", _pwc);
            info.AddValue("DX:CSC", _csc);
            info.AddValue("DX:R", _r);
            SerializationHelper.WriteList("DX:F", _f, info);
            SerializationHelper.WriteDictionary("DX:A", _a, info);
            SerializationHelper.WriteList("DX:J", _j, info);
        }

        private string _rg;

        private int _wc = 1;

        private int _pwi = 5000;

        private int _csi = 1000;

        private int _qs;

        private double _pwc;

        private double _csc;

        private Queue<SDR> _r = new Queue<SDR>();

        private List<EntityAspectReference<IEA, IEA>> _f = new List<EntityAspectReference<IEA, IEA>>();

        private Dictionary<Guid, List<EntityAspectReference<IEA, IEA>>> _a = new Dictionary<Guid, List<EntityAspectReference<IEA, IEA>>>();

        private List<Job> _j = new List<Job>();
    }
}
