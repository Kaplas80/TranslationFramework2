using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.EventArgs;
using UnderRailLib.TimelapseVertigo.Common.Data;
using UnderRailLib.TimelapseVertigo.Rules.Characters;
using UnderRailLib.TimelapseVertigo.Rules.Combat;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CO1")]
    [Serializable]
    public sealed class CO1 : CharacterCombatComponent
    {
        private CO1(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _cw = (WeaponItemInstance) info.GetValue("CO1:CW", typeof(WeaponItemInstance));
            if (DataModelVersion.MinorVersion >= 210)
            {
                _cdb = info.GetDouble("CO1:CDB");
            }
            else
            {
                _cdb = info.GetSingle("CO1:CDB");
            }

            if (DataModelVersion.MinorVersion >= 101)
            {
                _cc = info.GetDouble("CO1:CC");
            }
            else
            {
                _cc = Math.Round(info.GetSingle("CO1:CC"), 2);
            }

            _cwk = (WeaponKey) info.GetValue("CO1:CWK", typeof(WeaponKey));
            _ttns = info.GetDouble("CO1:TTNS");
            SerializationHelper.ReadDictionary("CO1:PO", ref _po, info);
            _p = (IBoundVariable<float>) info.GetValue("CO1:P", typeof(IBoundVariable<float>));
            _was = info.GetSingle("CO1:WAS");
            _ii = (InterruptibleAttackInfo) info.GetValue("CO1:II", typeof(InterruptibleAttackInfo));
            _rtc = info.GetDouble("CO1:RTC");
            _cws = (EventHandler<System.EventArgs>) info.GetValue("CO1:CWS", typeof(EventHandler<System.EventArgs>));
            if (DataModelVersion.MinorVersion >= 61)
            {
                _a = info.GetInt32("CO1:A");
            }

            if (DataModelVersion.MinorVersion >= 62)
            {
                SerializationHelper.ReadEvent("CO1:PA", ref _pa, info);
            }

            if (DataModelVersion.MinorVersion >= 75)
            {
                _nmp = info.GetInt32("CO1:NMP");
            }

            if (DataModelVersion.MinorVersion >= 87)
            {
                _f = info.GetDouble("CO1:F");
            }

            if (DataModelVersion.MinorVersion >= 110)
            {
                _x = info.GetDouble("CO1:X");
            }
            else
            {
                _x = 1.0;
            }

            if (DataModelVersion.MinorVersion >= 115)
            {
                _prm = info.GetDouble("CO1:PRM");
            }
            else
            {
                _prm = 1.0;
            }

            if (DataModelVersion.MinorVersion >= 118)
            {
                _pr = info.GetInt32("CO1:PR");
            }

            if (DataModelVersion.MinorVersion >= 134)
            {
                _b = info.GetInt32("CO1:B");
            }

            if (DataModelVersion.MinorVersion >= 135)
            {
                _h = info.GetDouble("CO1:H");
            }

            if (DataModelVersion.MinorVersion >= 166)
            {
                _sab = info.GetDouble("CO1:SAB");
            }

            if (DataModelVersion.MinorVersion >= 174)
            {
                _wm = info.GetDouble("CO1:WM");
            }

            if (DataModelVersion.MinorVersion >= 183)
            {
                _i = info.GetInt32("CO1:I");
            }

            if (DataModelVersion.MinorVersion >= 255)
            {
                _pb = info.GetInt32("CO1:PB");
                _prb = info.GetInt32("CO1:PRB");
            }

            if (DataModelVersion.MinorVersion >= 269)
            {
                _tpb = info.GetInt32("CO1:TPB");
            }

            if (DataModelVersion.MinorVersion >= 280)
            {
                _th = info.GetDouble("CO1:TH");
            }

            if (DataModelVersion.MinorVersion >= 358)
            {
                SerializationHelper.ReadDictionary("CO1:CM", ref _cm, info);
            }
            else
            {
                _cm = new Dictionary<string, CMD>();
            }

            if (DataModelVersion.MinorVersion >= 362)
            {
                _y = info.GetDouble("CO1:Y");
            }
            else
            {
                _y = 1.0;
            }

            if (DataModelVersion.MinorVersion >= 422)
            {
                _z = info.GetDouble("CO1:Z");
                return;
            }

            _z = 1.0;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CO1:CW", _cw);
            info.AddValue("CO1:CDB", _cdb);
            info.AddValue("CO1:CC", _cc);
            info.AddValue("CO1:CWK", _cwk);
            info.AddValue("CO1:TTNS", _ttns);
            info.AddValue("CO1:F", _f);
            SerializationHelper.WriteDictionary("CO1:PO", _po, info);
            info.AddValue("CO1:P", _p);
            info.AddValue("CO1:WAS", _was);
            info.AddValue("CO1:II", _ii);
            info.AddValue("CO1:RTC", _rtc);
            info.AddValue("CO1:CWS", _cws);
            info.AddValue("CO1:A", _a);
            info.AddValue("CO1:NMP", _nmp);
            info.AddValue("CO1:TH", _th);
            SerializationHelper.WriteEvent("CO1:PA", _pa, info);
            info.AddValue("CO1:X", _x);
            info.AddValue("CO1:PRM", _prm);
            info.AddValue("CO1:PB", _pb);
            info.AddValue("CO1:PRB", _prb);
            info.AddValue("CO1:PR", _pr);
            info.AddValue("CO1:B", _b);
            info.AddValue("CO1:H", _h);
            info.AddValue("CO1:SAB", _sab);
            info.AddValue("CO1:WM", _wm);
            info.AddValue("CO1:I", _i);
            info.AddValue("CO1:TPB", _tpb);
            SerializationHelper.WriteDictionary("CO1:CM", _cm, info);
            info.AddValue("CO1:Y", _y);
            info.AddValue("CO1:Z", _z);
        }

        private WeaponItemInstance _cw;

        private double _cdb;

        private double _cc;

        private WeaponKey _cwk;

        private double _ttns;

        private double _f;

        private Dictionary<PsiSchool, CharacterPsiOffense> _po = new Dictionary<PsiSchool, CharacterPsiOffense>();

        private IBoundVariable<float> _p;

        private int _nmp;

        private double _prm;

        private float _was;

        private int _a;

        private double _wm;

        private InterruptibleAttackInfo _ii;

        private double _rtc;

        private double _x = 1.0;

        private double _y = 1.0;

        private double _z = 1.0;

        private int _pb;

        private int _prb;

        private int _tpb;

        private int _pr;

        private int _b;

        private double _h;

        private double _th;

        private double _sab;

        private int _i;

        private Dictionary<string, CMD> _cm = new Dictionary<string, CMD>();

        private EventHandler<System.EventArgs> _cws;

        private EventHandler<eaP> _pa;
    }
}
