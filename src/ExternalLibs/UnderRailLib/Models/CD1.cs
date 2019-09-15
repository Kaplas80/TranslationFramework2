using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.EventArgs;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.TimelapseVertigo.Common.Data;
using UnderRailLib.TimelapseVertigo.Rules.Combat;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CD1")]
    [Serializable]
    public sealed class CD1 : CharacterCombatComponent, iSHD, ISerializable
    {
        private CD1(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _so = info.GetBoolean("CD1:SO");
            _d = info.GetInt32("CD1:D");
            _e = info.GetInt32("CD1:E");
            _f = info.GetInt32("CD1:F");
            _r = info.GetInt32("CD1:R");
            if (DataModelVersion.MinorVersion >= 91)
            {
                _e1 = info.GetDouble("CD1:E1");
            }
            else
            {
                _e1 = info.GetSingle("CD1:E1");
            }

            if (DataModelVersion.MinorVersion >= 88)
            {
                SerializationHelper.ReadDictionary("CD1:DR", ref _dr, info);
            }
            else
            {
                Dictionary<DamageType, int> dictionary = null;
                SerializationHelper.ReadDictionary("CD1:DR", ref dictionary, info);
                if (dictionary != null)
                {
                    _dr = new Dictionary<DamageType, DR2>();
                    foreach (KeyValuePair<DamageType, int> keyValuePair in dictionary)
                    {
                        _dr[keyValuePair.Key] = new DR2(keyValuePair.Key, keyValuePair.Value, 0.0);
                    }
                }
            }

            _rtc = info.GetDouble("CD1:RTC");
            _h = (IBoundVariable<float>) info.GetValue("CD1:H", typeof(IBoundVariable<float>));
            _hr = info.GetSingle("CD1:HR");
            _hm = info.GetSingle("CD1:HM");
            if (DataModelVersion.MajorVersion >= 5)
            {
                SerializationHelper.ReadEvent("CD1:ST", ref _st, info);
                SerializationHelper.ReadEvent("CD1:SH", ref _sh, info);
                SerializationHelper.ReadEvent("CD1:SVC", ref _svc, info);
                SerializationHelper.ReadEvent("CD1:SC", ref _sc, info);
                SerializationHelper.ReadEvent("CD1:CTD", ref _ctd, info);
                SerializationHelper.ReadEvent("CD1:CRD", ref _crd, info);
                SerializationHelper.ReadEvent("CD1:CED", ref _ced, info);
                SerializationHelper.ReadEvent("CD1:CSD", ref _csd, info);
                SerializationHelper.ReadEvent("CD1:CIM", ref _cim, info);
                SerializationHelper.ReadEvent("CD1:CSR", ref _csr, info);
            }
            else
            {
                _st = (EventHandler<eaPCEA>) info.GetValue("CD1:ST", typeof(EventHandler<eaPCEA>));
                _sh = (EventHandler<System.EventArgs>) info.GetValue("CD1:SH", typeof(EventHandler<System.EventArgs>));
                _svc = (EventHandler<eaPCEA>) info.GetValue("CD1:SVC", typeof(EventHandler<eaPCEA>));
                _sc = (EventHandler<System.EventArgs>) info.GetValue("CD1:SC", typeof(EventHandler<System.EventArgs>));
                _ctd = (EventHandler<eaC>) info.GetValue("CD1:CTD", typeof(EventHandler<eaC>));
                _crd = (EventHandler<eaC>) info.GetValue("CD1:CRD", typeof(EventHandler<eaC>));
                _ced = (EventHandler<eaC>) info.GetValue("CD1:CED", typeof(EventHandler<eaC>));
                _csd = (EventHandler<eaC>) info.GetValue("CD1:CSD", typeof(EventHandler<eaC>));
                _cim = (EventHandler<eaCAEA>) info.GetValue("CD1:CIM", typeof(EventHandler<eaCAEA>));
                _csr = (EventHandler<eaCSREA>) info.GetValue("CD1:CSR", typeof(EventHandler<eaCSREA>));
            }

            if (DataModelVersion.MajorVersion >= 19)
            {
                _dtm =
                    (info.GetValue("CD1:DTM", typeof(Dictionary<DamageType, double>)) as Dictionary<DamageType, double>);
            }
            else
            {
                _dtm = new Dictionary<DamageType, double>();
                foreach (var obj in Enum.GetValues(typeof(DamageType)))
                {
                    var key = (DamageType)obj;
                    _dtm[key] = 1.0;
                }
            }

            if (DataModelVersion.MajorVersion >= 62)
            {
                SerializationHelper.ReadEvent("CD1:CH", ref _ch, info);
            }

            if (DataModelVersion.MinorVersion >= 265)
            {
                SerializationHelper.ReadDictionary("CD1:M", ref _m, info);
            }
            else
            {
                _m = new Dictionary<AIM, ArmorItemInstance>();
            }

            if (DataModelVersion.MinorVersion >= 97)
            {
                _mrs = info.GetInt32("CD1:MRS");
            }

            if (DataModelVersion.MinorVersion >= 111)
            {
                _m2 = info.GetSingle("CD1:M");
            }
            else
            {
                _m2 = 1f;
            }

            if (DataModelVersion.MinorVersion >= 112)
            {
                _q = info.GetDouble("CD1:Q");
            }

            if (DataModelVersion.MinorVersion >= 151)
            {
                _hfx = info.GetDouble("CD1:HFX");
            }

            if (DataModelVersion.MinorVersion >= 164)
            {
                SerializationHelper.ReadEvent("CD1:CBD", ref _cbd, info);
            }

            if (DataModelVersion.MinorVersion >= 266)
            {
                _ed = info.GetDouble("CD1:ED");
            }
            else
            {
                _ed = 1.0;
            }

            if (DataModelVersion.MinorVersion >= 284)
            {
                _thm = info.GetDouble("CD1:THM");
                _thb = info.GetInt32("CD1:THB");
            }

            if (DataModelVersion.MinorVersion >= 379)
            {
                _vtd = (EventHandler<eaC>) info.GetValue("CD1:VTD", typeof(EventHandler<eaC>));
                _vrd = (EventHandler<eaC>) info.GetValue("CD1:VRD", typeof(EventHandler<eaC>));
            }

            if (DataModelVersion.MinorVersion >= 450)
            {
                _pcm = info.GetDouble("CD1:PCM");
            }

            if (DataModelVersion.MinorVersion >= 453)
            {
                _aef = info.GetDouble("CD1:AEF");
            }
            else
            {
                _aef = 1.0;
            }

            if (DataModelVersion.MinorVersion >= 485)
            {
                _pdt = info.GetDouble("CD1:PDT");
                return;
            }

            _pdt = 1.0;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CD1:SO", _so);
            info.AddValue("CD1:D", _d);
            info.AddValue("CD1:E", _e);
            info.AddValue("CD1:F", _f);
            info.AddValue("CD1:R", _r);
            info.AddValue("CD1:E1", _e1);
            SerializationHelper.WriteDictionary("CD1:DR", _dr, info);
            info.AddValue("CD1:RTC", _rtc);
            info.AddValue("CD1:H", _h);
            info.AddValue("CD1:HR", _hr);
            info.AddValue("CD1:HM", _hm);
            SerializationHelper.WriteEvent("CD1:ST", _st, info);
            SerializationHelper.WriteEvent("CD1:SH", _sh, info);
            SerializationHelper.WriteEvent("CD1:SVC", _svc, info);
            SerializationHelper.WriteEvent("CD1:SC", _sc, info);
            SerializationHelper.WriteEvent("CD1:CTD", _ctd, info);
            SerializationHelper.WriteEvent("CD1:CRD", _crd, info);
            SerializationHelper.WriteEvent("CD1:CBD", _cbd, info);
            SerializationHelper.WriteEvent("CD1:CED", _ced, info);
            SerializationHelper.WriteEvent("CD1:CSD", _csd, info);
            SerializationHelper.WriteEvent("CD1:CIM", _cim, info);
            SerializationHelper.WriteEvent("CD1:CSR", _csr, info);
            SerializationHelper.WriteEvent("CD1:CH", _ch, info);
            info.AddValue("CD1:DTM", _dtm);
            SerializationHelper.WriteDictionary("CD1:M", _m, info);
            info.AddValue("CD1:MRS", _mrs);
            info.AddValue("CD1:M", _m2);
            info.AddValue("CD1:Q", _q);
            info.AddValue("CD1:HFX", _hfx);
            info.AddValue("CD1:ED", _ed);
            info.AddValue("CD1:THM", _thm);
            info.AddValue("CD1:THB", _thb);
            SerializationHelper.WriteEvent("CD1:VTD", _vtd, info);
            SerializationHelper.WriteEvent("CD1:VRD", _vrd, info);
            info.AddValue("CD1:PCM", _pcm);
            info.AddValue("CD1:AEF", _aef);
            info.AddValue("CD1:PDT", _pdt);
        }

        private bool _so;

        private int _d;

        private int _e;

        private int _f;

        private int _r;

        private double _e1;

        private int _mrs;

        private Dictionary<DamageType, DR2> _dr = new Dictionary<DamageType, DR2>();

        private Dictionary<DamageType, double> _dtm = new Dictionary<DamageType, double>();

        private Dictionary<AIM, ArmorItemInstance> _m = new Dictionary<AIM, ArmorItemInstance>();

        private double _rtc;

        private IBoundVariable<float> _h;

        private float _hr;

        private float _hm;

        private double _thm;

        private int _thb;

        private float _m2;

        private double _q;

        private double _pcm;

        private double _hfx;

        private double _ed;

        private double _aef = 1.0;

        private double _pdt = 1.0;

        private EventHandler<eaPCEA> _st;

        private EventHandler<System.EventArgs> _sh;

        private EventHandler<eaPCEA> _svc;

        private EventHandler<System.EventArgs> _sc;

        private EventHandler<eaC> _ctd;

        private EventHandler<eaC> _crd;

        private EventHandler<eaC> _cbd;

        private EventHandler<eaC> _ced;

        private EventHandler<eaC> _csd;

        private EventHandler<eaCAEA> _cim;

        private EventHandler<eaCSREA> _csr;

        private EventHandler<eaCH> _ch;

        private EventHandler<eaC> _vtd;

        private EventHandler<eaC> _vrd;
    }
}
