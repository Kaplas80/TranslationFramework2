using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.TimelapseVertigo.Rules.Combat;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("WI")]
    [Serializable]
    public sealed class WeaponItem : EquippableItem
    {
        private WeaponItem(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _weaponType = (WeaponType) info.GetValue("WI:WT", typeof(WeaponType));
            _s = info.GetSingle("WI:S");
            SerializationHelper.ReadList("WI:D", ref _damage, info);
            _impactSpeed = (ImpactSpeed) info.GetValue("WI:IS", typeof(ImpactSpeed));
            _ms = info.GetInt32("WI:MS");
            _mr = info.GetSingle("WI:MR");
            _or = info.GetSingle("WI:OR");
            if (DataModelVersion.MinorVersion >= 101)
            {
                _csc = info.GetDouble("WI:CSC");
            }
            else
            {
                _csc = Math.Round(info.GetSingle("WI:CSC"), 2);
            }

            SerializationHelper.ReadList("WI:OHE", ref _ohe, info);
            _wvm = info.GetString("WI:WVM");
            if (DataModelVersion.MinorVersion >= 24)
            {
                _isndm = (eISM) info.GetValue("WI:ISNDM", typeof(eISM));
            }

            if (DataModelVersion.MinorVersion >= 50)
            {
                _p = (eWPM) info.GetValue("WI:P", typeof(eWPM));
            }

            if (DataModelVersion.MinorVersion >= 51)
            {
                _ammoType = (AmmoType) info.GetValue("WI:A", typeof(AmmoType));
                _m = info.GetInt32("WI:M");
            }

            if (DataModelVersion.MinorVersion >= 54)
            {
                _au = info.GetDouble("WI:AU");
            }

            if (DataModelVersion.MinorVersion >= 60)
            {
                _ap = info.GetInt32("WI:AP");
            }
            else
            {
                _ap = 15;
            }

            if (DataModelVersion.MinorVersion >= 66)
            {
                if (DataModelVersion.MinorVersion >= 131)
                {
                    _pb = info.GetDouble("WI:PB");
                }
                else
                {
                    _pb = Math.Round(info.GetSingle("WI:PB"), 2);
                }
            }

            if (DataModelVersion.MinorVersion >= 81)
            {
                _pp = info.GetSingle("WI:PP");
            }

            if (DataModelVersion.MinorVersion >= 87)
            {
                _fp = info.GetDouble("WI:FP");
            }

            if (DataModelVersion.MinorVersion >= 98)
            {
                _bp = info.GetDouble("WI:BP");
            }

            if (DataModelVersion.MinorVersion >= 175)
            {
                _cdb = info.GetDouble("WI:CDB");
            }
            else
            {
                _cdb = 1.0;
            }

            if (DataModelVersion.MinorVersion >= 181)
            {
                _fd = info.GetBoolean("WI:FD");
            }

            if (DataModelVersion.MinorVersion >= 200)
            {
                _sb = (info.GetValue("WI:SB", typeof(double?)) as double?);
            }

            if (DataModelVersion.MinorVersion >= 223)
            {
                _ia = info.GetBoolean("WI:IA");
            }

            if (DataModelVersion.MinorVersion >= 260)
            {
                _eb = info.GetInt32("WI:EB");
            }

            if (DataModelVersion.MinorVersion >= 360)
            {
                _asa = (info.GetValue("WI:ASA", typeof(int?)) as int?);
            }

            if (DataModelVersion.MinorVersion >= 361 && DataModelVersion.MinorVersion < 365)
            {
                var sa = new List<CapabilityReference>();
                SerializationHelper.ReadList("WI:SA", ref sa, info);
                _qc = sa;
            }

            if (DataModelVersion.MinorVersion >= 390)
            {
                _wue = (eWUE) info.GetValue("WI:WUE", typeof(eWUE));
            }

            if (DataModelVersion.MinorVersion >= 418)
            {
                _rac = info.GetDouble("WI:RAC");
            }

            if (DataModelVersion.MinorVersion >= 458)
            {
                SerializationHelper.ReadList("WI:OHC", ref _ohc, info);
            }
            else
            {
                _ohc = new List<string>();
            }

            if (DataModelVersion.MinorVersion >= 471)
            {
                _weaponSubtype = (info.GetValue("WI:WST", typeof(WeaponSubtype?)) as WeaponSubtype?);
                _csa = info.GetString("WI:CSA");
                _df = info.GetBoolean("WI:DF");
                _ctbd = (info.GetValue("WI:CTBD", typeof(int?)) as int?);
                _caa = info.GetString("WI:CAA");
            }

            if (DataModelVersion.MinorVersion >= 517)
            {
                SerializationHelper.ReadList("WI:WAM", ref _wam, info);
            }
            else
            {
                _wam = new List<WAM>();
            }

            if (DataModelVersion.MinorVersion >= 518)
            {
                _mrb = info.GetInt32("WI:MRB");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("WI:WT", _weaponType);
            info.AddValue("WI:S", _s);
            SerializationHelper.WriteList("WI:D", _damage, info);
            info.AddValue("WI:IS", _impactSpeed);
            info.AddValue("WI:MS", _ms);
            info.AddValue("WI:MR", _mr);
            info.AddValue("WI:OR", _or);
            info.AddValue("WI:CSC", _csc);
            SerializationHelper.WriteList("WI:OHE", _ohe, info);
            info.AddValue("WI:P", _p);
            info.AddValue("WI:WVM", _wvm);
            info.AddValue("WI:ISNDM", _isndm);
            info.AddValue("WI:A", _ammoType);
            info.AddValue("WI:M", _m);
            info.AddValue("WI:AU", _au);
            info.AddValue("WI:AP", _ap);
            info.AddValue("WI:PB", _pb);
            info.AddValue("WI:PP", _pp);
            info.AddValue("WI:FP", _fp);
            info.AddValue("WI:BP", _bp);
            info.AddValue("WI:CDB", _cdb);
            info.AddValue("WI:FD", _fd);
            if (DataModelVersion.MinorVersion >= 200)
            {
                info.AddValue("WI:SB", _sb);
            }

            if (DataModelVersion.MinorVersion >= 223)
            {
                info.AddValue("WI:IA", _ia);
            }

            if (DataModelVersion.MinorVersion >= 260)
            {
                info.AddValue("WI:EB", _eb);
            }

            if (DataModelVersion.MinorVersion >= 360)
            {
                info.AddValue("WI:ASA", _asa);
            }

            if (DataModelVersion.MinorVersion >= 390)
            {
                info.AddValue("WI:WUE", _wue);
            }

            if (DataModelVersion.MinorVersion >= 418)
            {
                info.AddValue("WI:RAC", _rac);
            }

            if (DataModelVersion.MinorVersion >= 458)
            {
                SerializationHelper.WriteList("WI:OHC", _ohc, info);
            }

            if (DataModelVersion.MinorVersion >= 471)
            {
                info.AddValue("WI:WST", _weaponSubtype);
                info.AddValue("WI:CSA", _csa);
                info.AddValue("WI:DF", _df);
                info.AddValue("WI:CTBD", _ctbd);
                info.AddValue("WI:CAA", _caa);
            }

            if (DataModelVersion.MinorVersion >= 517)
            {
                SerializationHelper.WriteList("WI:WAM", _wam, info);
            }

            if (DataModelVersion.MinorVersion >= 518)
            {
                info.AddValue("WI:MRB", _mrb);
            }
        }

        private WeaponType _weaponType;

        private float _s = 1f;

        private int _ap = 15;

        private List<DR> _damage = new List<DR>();

        private bool _fd;

        private ImpactSpeed _impactSpeed;

        private int _ms;

        private float _mr;

        private float _or;

        private int _mrb;

        private double _csc = 0.05;

        private double _cdb = 1.0;

        private List<HitEffectReference> _ohe = new List<HitEffectReference>();

        private AmmoType _ammoType;

        private bool _ia;

        private int _m = 10;

        private double _au;

        private double _pb;

        private double _bp;

        private int _eb;

        private float _pp;

        private double _fp;

        private double? _sb;

        private int? _asa;

        private double _rac = 1.0;

        private string _wvm;

        private eISM _isndm;

        private eWPM _p;

        private eWUE _wue;

        private List<string> _ohc = new List<string>();

        private WeaponSubtype? _weaponSubtype;

        private string _csa;

        private bool _df;

        private int? _ctbd;

        private string _caa;

        private List<WAM> _wam = new List<WAM>();
    }
}