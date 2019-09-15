using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Aspects;
using UnderRailLib.Ouroboros.Common.Data;

namespace UnderRailLib.Models
{
    [EncodedTypeName("IEA")]
    [Serializable]
    public sealed class IEA : Aspect<LE2>, iINA, iMAIA, b8i
    {
        private IEA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _c = (PropertyCollection)info.GetValue("IEA:C", typeof(PropertyCollection));
            _b = info.GetString("IEA:B");
            SerializationHelper.ReadList("IEA:S", ref _s, info);
            _psi = info.GetInt32("IEA:PSI");
            _gsi = info.GetInt32("IEA:GSI");
            _ri = info.GetInt32("IEA:RI");
            SerializationHelper.ReadList("IEA:CBP", ref _cbp, info);
            _wc = (StandaloneWorkflowContainer)info.GetValue("IEA:WC", typeof(StandaloneWorkflowContainer));
            _wqfs = info.GetBoolean("IEA:WQFS");
            _wii = info.GetBoolean("IEA:WII");
            if (DataModelVersion.MinorVersion >= 58)
            {
                _t = info.GetString("IEA:T");
            }
            if (DataModelVersion.MinorVersion >= 149)
            {
                _e = info.GetBoolean("IEA:E");
            }
            else
            {
                _e = true;
            }
            if (DataModelVersion.MinorVersion >= 168)
            {
                _tp = (PropertyCollection)info.GetValue("IEA:TP", typeof(PropertyCollection));
            }
            else
            {
                _tp = new PropertyCollection();
            }
            if (DataModelVersion.MinorVersion >= 243)
            {
                _cs = info.GetString("IEA:CS");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("IEA:C", _c);
            info.AddValue("IEA:B", _b);
            SerializationHelper.WriteList("IEA:S", _s, info);
            info.AddValue("IEA:PSI", _psi);
            info.AddValue("IEA:GSI", _gsi);
            info.AddValue("IEA:RI", _ri);
            SerializationHelper.WriteList("IEA:CBP", _cbp, info);
            info.AddValue("IEA:WC", _wc);
            info.AddValue("IEA:WQFS", _wqfs);
            info.AddValue("IEA:WII", _wii);
            info.AddValue("IEA:T", _t);
            info.AddValue("IEA:E", _e);
            info.AddValue("IEA:TP", _tp);
            info.AddValue("IEA:CS", _cs);
        }

        private PropertyCollection _c = new PropertyCollection();

        private string _b;

        private string _t;

        private PropertyCollection _tp = new PropertyCollection();

        private List<ScannerInfo> _s = new List<ScannerInfo>();

        private int _psi;

        private int _gsi;

        private int _ri;

        private List<string> _cbp = new List<string>();

        private StandaloneWorkflowContainer _wc;

        private bool _wqfs;

        private bool _wii;

        private bool _e = true;

        private string _cs;
    }
}
