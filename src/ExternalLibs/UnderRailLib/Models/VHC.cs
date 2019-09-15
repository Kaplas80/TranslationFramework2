using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Ouroboros.Common.Data;
using UnderRailLib.TimelapseVertigo.Rules.Combat;

namespace UnderRailLib.Models
{
    [EncodedTypeName("VHC")]
    [Serializable]
    public sealed class VHC : ISerializable
    {
        private VHC(SerializationInfo info, StreamingContext ctx)
        {
            _df = (info.GetValue("VHC:DF", typeof(TypedObjectReference<VHCD>)) as TypedObjectReference<VHCD>);
            SerializationHelper.ReadDictionary("VHC:P", ref _p, info);
            _t = (info.GetValue("VHC:T", typeof(ItemContainer)) as ItemContainer);
            _d = (info.GetValue("VHC:D", typeof(C1)) as C1);
            _v = (eVT) info.GetValue("VHC:V", typeof(eVT));
            _s = info.GetDouble("VHC:S");
            _md = info.GetInt32("VHC:MD");
            _mb = info.GetInt32("VHC:MB");
            SerializationHelper.ReadDictionary("VHC:R", ref _r, info);
            _a = (eCAT) info.GetValue("VHC:A", typeof(eCAT));
            _c = info.GetDouble("VHC:C");
            _st = info.GetDouble("VHC:ST");
            _sm = info.GetDouble("VHC:SM");
            _vw = info.GetDouble("VHC:VW");
            _tw = info.GetDouble("VHC:TW");
            _ep = info.GetInt32("VHC:EP");
            _ec = info.GetInt32("VHC:EC");
            SerializationHelper.ReadList("VHC:SN", ref _sn, info);
            _ms = info.GetDouble("VHC:MS");
            _y = info.GetDouble("VHC:ECT");
            _dr = info.GetDouble("VHC:DR");
            _f = info.GetString("VHC:VM");
            if (DataModelVersion.MinorVersion >= 378)
            {
                SerializationHelper.ReadList("VHC:PF", ref _pf, info);
                _kc = info.GetString("VHC:KC");
                _ls = info.GetInt32("VHC:LS");
            }
            else
            {
                _pf = new List<string>();
            }

            if (DataModelVersion.MinorVersion >= 382)
            {
                _ils = info.GetInt32("VHC:ILS");
                _il = info.GetBoolean("VHC:IL");
            }
            else
            {
                _ils = 50;
            }

            if (DataModelVersion.MinorVersion >= 386)
            {
                SerializationHelper.ReadList("VHC:CP", ref _cp, info);
            }
            else
            {
                _cp = new List<CapabilityReference>();
            }

            if (DataModelVersion.MinorVersion >= 506)
            {
                _e = info.GetDouble("VHC:E");
            }
            else
            {
                _e = 1.0;
            }

            if (DataModelVersion.MinorVersion >= 511)
            {
                _m = info.GetDouble("VHC:M");
            }
            else
            {
                _m = 0.5;
            }

            if (DataModelVersion.MinorVersion >= 512)
            {
                _mbd = info.GetDouble("VHC:MBD");
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("VHC:DF", _df);
            SerializationHelper.WriteDictionary("VHC:P", _p, info);
            info.AddValue("VHC:T", _t);
            info.AddValue("VHC:D", _d);
            info.AddValue("VHC:V", _v);
            info.AddValue("VHC:S", _s);
            info.AddValue("VHC:MD", _md);
            info.AddValue("VHC:MB", _mb);
            SerializationHelper.WriteDictionary("VHC:R", _r, info);
            info.AddValue("VHC:A", _a);
            info.AddValue("VHC:C", _c);
            info.AddValue("VHC:ST", _st);
            info.AddValue("VHC:SM", _sm);
            info.AddValue("VHC:VW", _vw);
            info.AddValue("VHC:TW", _tw);
            info.AddValue("VHC:EP", _ep);
            info.AddValue("VHC:EC", _ec);
            SerializationHelper.WriteList("VHC:SN", _sn, info);
            info.AddValue("VHC:MS", _ms);
            info.AddValue("VHC:ECT", _y);
            info.AddValue("VHC:DR", _dr);
            info.AddValue("VHC:VM", _f);
            SerializationHelper.WriteList("VHC:PF", _pf, info);
            info.AddValue("VHC:KC", _kc);
            info.AddValue("VHC:LS", _ls);
            info.AddValue("VHC:ILS", _ils);
            info.AddValue("VHC:IL", _il);
            SerializationHelper.WriteList("VHC:CP", _cp, info);
            info.AddValue("VHC:E", _e);
            info.AddValue("VHC:M", _m);
            info.AddValue("VHC:MBD", _mbd);
        }

        private TypedObjectReference<VHCD> _df;

        private Dictionary<string, VehiclePartItemInstance> _p = new Dictionary<string, VehiclePartItemInstance>();

        private ItemContainer _t;

        private C1 _d;

        private string _f;

        private eVT _v;

        private double _s;

        private double _e;

        private double _mbd;

        private int _md;

        private int _mb;

        private Dictionary<DamageType, DR2> _r = new Dictionary<DamageType, DR2>();

        private eCAT _a;

        private double _c;

        private double _st;

        private double _m;

        private double _sm = 1.0;

        private double _vw;

        private double _tw;

        private int _ep;

        private int _ec;

        private List<VHS> _sn = new List<VHS>();

        private double _ms;

        private double _y;

        private double _dr;

        private List<string> _pf = new List<string>();

        private string _kc;

        private int _ls;

        private int _ils;

        private bool _il;

        private List<CapabilityReference> _cp = new List<CapabilityReference>();
    }
}
