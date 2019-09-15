using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Models.EventArgs;
using UnderRailLib.Ouroboros.Common.Data;
using UnderRailLib.TimelapseVertigo.Rules.Capabilities;
using UnderRailLib.TimelapseVertigo.Rules.Characters;

namespace UnderRailLib.Models
{
    [EncodedTypeName("C1")]
    [Serializable]
    public class C1 : ISerializable
    {
        protected C1(SerializationInfo info, StreamingContext ctx)
        {
            _i = info.GetString("C1:I");
            _u = (Guid) info.GetValue("C1:U", typeof(Guid));
            _n = info.GetString("C1:N");
            _l = info.GetInt32("C1:L");
            _p = info.GetBoolean("C1:P");
            _d = info.GetBoolean("C1:D");
            _ss = (CharacterSpecialStates) info.GetValue("C1:SS", typeof(CharacterSpecialStates));
            _c = (Color) info.GetValue("C1:C", typeof(Color));
            _ba = (BaseAbilities) info.GetValue("C1:BA", typeof(BaseAbilities));
            _s = (Skills) info.GetValue("C1:S", typeof(Skills));
            f = (Feats) info.GetValue("C1:F", typeof(Feats));
            _capabilitySetAttacks = (CapabilitySet) info.GetValue("C1:SA", typeof(CapabilitySet));
            _capabilitySetAbilities = (CapabilitySet) info.GetValue("C1:SA1", typeof(CapabilitySet));
            _capabilitySetPsi = (CapabilitySet) info.GetValue("C1:PA", typeof(CapabilitySet));
            _c1 = (CM1) info.GetValue("C1:C1", typeof(CM1));
            _se = (CharacterStatusEffects) info.GetValue("C1:SE", typeof(CharacterStatusEffects));
            _a1 = (Auras) info.GetValue("C1:A1", typeof(Auras));
            _b = (ItemContainer) info.GetValue("C1:B", typeof(ItemContainer));
            _g = (CharacterGearSet) info.GetValue("C1:G", typeof(CharacterGearSet));
            _ms = info.GetSingle("C1:MS");
            _bms = info.GetSingle("C1:BMS");
            _nt = info.GetBoolean("C1:NT");
            _t = info.GetBoolean("C1:T");
            _nd = info.GetBoolean("C1:ND");
            _d1 = info.GetBoolean("C1:D1");
            _aqs = info.GetInt32("C1:AQS");
            _d2 = (CD1) info.GetValue("C1:D2", typeof(CD1));
            _o = (CO1) info.GetValue("C1:O", typeof(CO1));
            SerializationHelper.ReadDictionary("C1:DW", ref _dw, info);
            SerializationHelper.ReadDictionary("C1:PA1", ref _pa1, info);
            _ctl = info.GetDouble("C1:CTL");
            _sr = info.GetSingle("C1:SR");
            _bsr = info.GetSingle("C1:BSR");
            _fi = info.GetString("C1:FI");
            _hbm = (eFBCM) info.GetValue("C1:HBM", typeof(eFBCM));
            _pe = info.GetBoolean("C1:PE");
            _bdm = info.GetSingle("C1:BDM");
            _dm = info.GetSingle("C1:DM");
            _d3 = info.GetInt32("C1:D3");
            _a2 = (eCAT) info.GetValue("C1:A2", typeof(eCAT));
            _ab = (eCAT) info.GetValue("C1:AB", typeof(eCAT));
            _i1 = (eCAI) info.GetValue("C1:I1", typeof(eCAI));
            _ib = (eCAI) info.GetValue("C1:IB", typeof(eCAI));
            if (DataModelVersion.MinorVersion >= 0)
            {
                SerializationHelper.ReadEvent("C1:R", ref _r, info);
                SerializationHelper.ReadEvent("C1:D4", ref _d4, info);
                SerializationHelper.ReadEvent("C1:CSC", ref _csc, info);
            }
            else
            {
                _r = (EventHandler<System.EventArgs>) info.GetValue("C1:R", typeof(EventHandler<System.EventArgs>));
                _d4 = (EventHandler<eaCAEA>) info.GetValue("C1:D4", typeof(EventHandler<eaCAEA>));
            }

            if (DataModelVersion.MinorVersion >= 47)
            {
                _g1 = (Gender) info.GetValue("C1:G1", typeof(Gender));
            }
            else
            {
                _g1 = Gender.Male;
            }

            if (DataModelVersion.MinorVersion >= 132)
            {
                _ett = info.GetDouble("C1:ETT");
            }

            if (DataModelVersion.MinorVersion >= 157)
            {
                _sp = info.GetDouble("C1:SP");
            }

            if (DataModelVersion.MinorVersion >= 178)
            {
                SerializationHelper.ReadDictionary("C1:CMP", ref _cmp, info);
                SerializationHelper.ReadDictionary("C1:CMF", ref _cmf, info);
            }
            else
            {
                _cmp = new Dictionary<CooldownCategory, double>();
                _cmf = new Dictionary<CooldownCategory, int>();
            }

            if (DataModelVersion.MinorVersion >= 192)
            {
                _cc = info.GetInt32("C1:CC");
            }

            if (DataModelVersion.MinorVersion >= 193)
            {
                _el = (eEL) info.GetValue("C1:EL", typeof(eEL));
            }

            if (DataModelVersion.MinorVersion >= 250)
            {
                _p2 = info.GetString("C1:P2");
            }

            if (DataModelVersion.MinorVersion >= 252)
            {
                _ca = (info.GetValue("C1:CA", typeof(PropertyCollection)) as PropertyCollection);
            }
            else
            {
                _ca = new PropertyCollection();
            }

            if (DataModelVersion.MinorVersion >= 267)
            {
                SerializationHelper.ReadDictionary("C1:SCM", ref _scm, info);
            }
            else
            {
                _scm = new Dictionary<SpecialCategory, XMD>();
            }

            if (DataModelVersion.MinorVersion >= 294)
            {
                _e = info.GetBoolean("C1:E");
            }

            if (DataModelVersion.MinorVersion >= 349)
            {
                _e2 = info.GetBoolean("C1:E2");
            }
            else
            {
                _e2 = _e;
            }

            if (DataModelVersion.MinorVersion >= 365)
            {
                _capabilitySetGear = (CapabilitySet) info.GetValue("C1:QC", typeof(CapabilitySet));
            }

            if (DataModelVersion.MinorVersion >= 376)
            {
                _vh = (info.GetValue("C1:VH", typeof(VHC)) as VHC);
            }

            if (DataModelVersion.MinorVersion >= 391)
            {
                SerializationHelper.ReadDictionary("C1:SPC", ref _spc, info);
                return;
            }

            _spc = new Dictionary<string, SNF>();
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("C1:I", _i);
            info.AddValue("C1:U", _u);
            info.AddValue("C1:N", _n);
            info.AddValue("C1:G1", _g1);
            info.AddValue("C1:L", _l);
            info.AddValue("C1:P", _p);
            info.AddValue("C1:D", _d);
            info.AddValue("C1:SS", _ss);
            info.AddValue("C1:C", _c);
            info.AddValue("C1:P2", _p2);
            info.AddValue("C1:BA", _ba);
            info.AddValue("C1:S", _s);
            info.AddValue("C1:F", f);
            info.AddValue("C1:SA", _capabilitySetAttacks);
            info.AddValue("C1:SA1", _capabilitySetAbilities);
            info.AddValue("C1:PA", _capabilitySetPsi);
            info.AddValue("C1:C1", _c1);
            info.AddValue("C1:SE", _se);
            info.AddValue("C1:A1", _a1);
            info.AddValue("C1:B", _b);
            info.AddValue("C1:G", _g);
            info.AddValue("C1:MS", _ms);
            info.AddValue("C1:BMS", _bms);
            info.AddValue("C1:NT", _nt);
            info.AddValue("C1:T", _t);
            info.AddValue("C1:ND", _nd);
            info.AddValue("C1:D1", _d1);
            info.AddValue("C1:AQS", _aqs);
            info.AddValue("C1:D2", _d2);
            info.AddValue("C1:O", _o);
            SerializationHelper.WriteDictionary("C1:DW", _dw, info);
            SerializationHelper.WriteDictionary("C1:PA1", _pa1, info);
            info.AddValue("C1:CTL", _ctl);
            info.AddValue("C1:SR", _sr);
            info.AddValue("C1:BSR", _bsr);
            info.AddValue("C1:FI", _fi);
            info.AddValue("C1:HBM", _hbm);
            info.AddValue("C1:PE", _pe);
            info.AddValue("C1:BDM", _bdm);
            info.AddValue("C1:DM", _dm);
            info.AddValue("C1:D3", _d3);
            info.AddValue("C1:A2", _a2);
            info.AddValue("C1:AB", _ab);
            info.AddValue("C1:I1", _i1);
            info.AddValue("C1:IB", _ib);
            info.AddValue("C1:ETT", _ett);
            info.AddValue("C1:SP", _sp);
            SerializationHelper.WriteEvent("C1:R", _r, info);
            SerializationHelper.WriteEvent("C1:D4", _d4, info);
            SerializationHelper.WriteEvent("C1:CSC", _csc, info);
            SerializationHelper.WriteDictionary("C1:CMP", _cmp, info);
            SerializationHelper.WriteDictionary("C1:CMF", _cmf, info);
            info.AddValue("C1:CC", _cc);
            info.AddValue("C1:EL", _el);
            info.AddValue("C1:CA", _ca);
            SerializationHelper.WriteDictionary("C1:SCM", _scm, info);
            info.AddValue("C1:E", _e);
            info.AddValue("C1:E2", _e2);
            info.AddValue("C1:QC", _capabilitySetGear);
            info.AddValue("C1:VH", _vh);
            SerializationHelper.WriteDictionary("C1:SPC", _spc, info);
        }

        private string _i;

        private Guid _u;

        private string _n;

        private Gender _g1;

        private int _l;

        private bool _p;

        private bool _e;

        private bool _e2;

        private bool _d;

        private CharacterSpecialStates _ss;

        private Color _c;

        private string _p2;

        private BaseAbilities _ba;

        private Skills _s;

        private Feats f;

        private CapabilitySet _capabilitySetAttacks;

        private CapabilitySet _capabilitySetAbilities;

        private CapabilitySet _capabilitySetPsi;

        private CapabilitySet _capabilitySetGear;

        private CM1 _c1;

        private CharacterStatusEffects _se;

        private Auras _a1;

        private ItemContainer _b;

        private CharacterGearSet _g;

        private float _ms;

        private float _bms = 1f;

        private bool _nt;

        private bool _t;

        private bool _nd;

        private bool _d1;

        private int _aqs;

        private CD1 _d2;

        private CO1 _o;

        private Dictionary<WeaponKey, WeaponItemInstance> _dw = new Dictionary<WeaponKey, WeaponItemInstance>();

        private Dictionary<string, eATTI> _pa1 = new Dictionary<string, eATTI>();

        private double _ctl;

        private double _ett;

        private float _sr;

        private float _bsr;

        private string _fi;

        private eFBCM _hbm;

        private bool _pe;

        private float _bdm = 1f;

        private float _dm;

        private int _d3;

        private double _sp;

        private eCAT _a2;

        private eCAT _ab;

        private eCAI _i1;

        private eCAI _ib;

        private Dictionary<CooldownCategory, double> _cmp = new Dictionary<CooldownCategory, double>();

        private Dictionary<CooldownCategory, int> _cmf = new Dictionary<CooldownCategory, int>();

        private Dictionary<SpecialCategory, XMD> _scm = new Dictionary<SpecialCategory, XMD>();

        private int _cc;

        private eEL _el;

        private PropertyCollection _ca = new PropertyCollection();

        private VHC _vh;

        private Dictionary<string, SNF> _spc = new Dictionary<string, SNF>();

        private EventHandler<System.EventArgs> _r;

        private EventHandler<eaCAEA> _d4;

        private EventHandler<System.EventArgs> _csc;
    }
}
