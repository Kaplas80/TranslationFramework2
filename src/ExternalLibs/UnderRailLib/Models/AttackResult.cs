using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Ouroboros.Common.Data;
using UnderRailLib.TimelapseVertigo.Rules.Combat;
using UnderRailLib.TimelapseVertigo.Rules.Items;

namespace UnderRailLib.Models
{
    [EncodedTypeName("AR2")]
    [Serializable]
    public sealed class AttackResult : ISerializable
    {
        private AttackResult(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _hit = info.GetBoolean("AR2:H");
                _criticalHit = info.GetBoolean("AR2:CH");
                SerializationHelper.ReadList("AR2:D", ref _damages, info);
                SerializationHelper.ReadList("AR2:OHE", ref _onHitEffects, info);
                _impactSpeed = (ImpactSpeed) info.GetValue("AR2:IS", typeof(ImpactSpeed));
                _attacker = (CharacterInfo) info.GetValue("AR2:A", typeof(CharacterInfo));
                _target = (CharacterInfo) info.GetValue("AR2:T", typeof(CharacterInfo));
                _medium = info.GetString("AR2:M");
                _bypassShield = info.GetBoolean("AR2:BS");
                _bypassResistance = info.GetBoolean("AR2:BR");
                _evadeable = info.GetBoolean("AR2:E");
                _periodic = info.GetBoolean("AR2:P");
                _noPushback = info.GetBoolean("AR2:NP");
                _pushbackModifier = info.GetDouble("AR2:PM");
                _tg = (eART) info.GetValue("AR2:TG", typeof(eART));
                _s = info.GetBoolean("AR2:S");
                _el = info.GetString("AR2:EL");
                _i = info.GetBoolean("AR2:I");
                if (DataModelVersion.MinorVersion >= 65)
                {
                    _y = (AmmoType) info.GetValue("AR2:Y", typeof(AmmoType));
                }

                if (DataModelVersion.MinorVersion >= 76)
                {
                    _cm = info.GetString("AR2:CM");
                }

                if (DataModelVersion.MinorVersion >= 92)
                {
                    _ml = info.GetBoolean("AR2:ML");
                }

                if (DataModelVersion.MinorVersion >= 93)
                {
                    SerializationHelper.ReadList("AR:X", ref _x, info);
                }

                if (DataModelVersion.MinorVersion >= 114)
                {
                    _u = info.GetBoolean("AR2:U");
                }

                if (DataModelVersion.MinorVersion >= 159)
                {
                    _c = (info.GetValue("AR2:C", typeof(object[])) as object[]);
                }

                if (DataModelVersion.MinorVersion >= 164)
                {
                    _b = info.GetBoolean("AR2:B");
                }

                if (DataModelVersion.MinorVersion >= 170)
                {
                    _o = (info.GetValue("AR2:O", typeof(WeaponSubtype?)) as WeaponSubtype?);
                    _cp = (info.GetValue("AR2:CP", typeof(PropertyCollection)) as PropertyCollection);
                }
                else
                {
                    _cp = new PropertyCollection();
                }

                if (DataModelVersion.MinorVersion >= 172)
                {
                    _sa = (Guid) info.GetValue("AR2:SA", typeof(Guid));
                }

                if (DataModelVersion.MinorVersion >= 200)
                {
                    _sb = info.GetDouble("AR2:SB");
                }

                if (DataModelVersion.MinorVersion >= 279)
                {
                    _bt = info.GetBoolean("AR2:BT");
                }

                if (DataModelVersion.MinorVersion >= 343)
                {
                    _rb = info.GetDouble("AR2:RB");
                }

                if (DataModelVersion.MinorVersion >= 380)
                {
                    _hv = (info.GetValue("AR2:HV", typeof(bool?)) as bool?);
                }

                if (DataModelVersion.MinorVersion >= 485)
                {
                    _ip = info.GetBoolean("AR2:IP");
                }

                if (DataModelVersion.MinorVersion >= 527)
                {
                    _nl = info.GetBoolean("AR2:NL");
                }

                if (DataModelVersion.MinorVersion >= 529)
                {
                    _cw = (info.GetValue("AR2:CW", typeof(WeaponItemInstance)) as WeaponItemInstance);
                }

                if (DataModelVersion.MinorVersion >= 547)
                {
                    _am = info.GetString("AR2:AM");
                }
            }
            else
            {
                if (GetType() == typeof(AttackResult))
                {
                    _hit = info.GetBoolean("Hit");
                    _criticalHit = info.GetBoolean("CriticalHit");
                    _damages = (List<Damage>) info.GetValue("Damages", typeof(List<Damage>));
                    _onHitEffects = (List<HitEffectReference>) info.GetValue("OnHitEffects", typeof(List<HitEffectReference>));
                    _impactSpeed = (ImpactSpeed) info.GetValue("ImpactSpeed", typeof(ImpactSpeed));
                    _attacker = (CharacterInfo) info.GetValue("Attacker", typeof(CharacterInfo));
                    _target = (CharacterInfo) info.GetValue("Target", typeof(CharacterInfo));
                    _medium = info.GetString("Medium");
                    _bypassShield = info.GetBoolean("BypassShield");
                    _bypassResistance = info.GetBoolean("BypassResistance");
                    _evadeable = info.GetBoolean("Evadeable");
                    _periodic = info.GetBoolean("Periodic");
                    _noPushback = info.GetBoolean("NoPushback");
                    _pushbackModifier = info.GetDouble("PushbackModifier");
                    return;
                }

                _hit = info.GetBoolean("AttackResult+Hit");
                _criticalHit = info.GetBoolean("AttackResult+CriticalHit");
                _damages = (List<Damage>) info.GetValue("AttackResult+Damages", typeof(List<Damage>));
                _onHitEffects = (List<HitEffectReference>) info.GetValue("AttackResult+OnHitEffects", typeof(List<HitEffectReference>));
                _impactSpeed = (ImpactSpeed) info.GetValue("AttackResult+ImpactSpeed", typeof(ImpactSpeed));
                _attacker = (CharacterInfo) info.GetValue("AttackResult+Attacker", typeof(CharacterInfo));
                _target = (CharacterInfo) info.GetValue("AttackResult+Target", typeof(CharacterInfo));
                _medium = info.GetString("AttackResult+Medium");
                _bypassShield = info.GetBoolean("AttackResult+BypassShield");
                _bypassResistance = info.GetBoolean("AttackResult+BypassResistance");
                _evadeable = info.GetBoolean("AttackResult+Evadeable");
                _periodic = info.GetBoolean("AttackResult+Periodic");
                _noPushback = info.GetBoolean("AttackResult+NoPushback");
                _pushbackModifier = info.GetDouble("AttackResult+PushbackModifier");
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("AR2:H", _hit);
            info.AddValue("AR2:CH", _criticalHit);
            SerializationHelper.WriteList("AR2:D", _damages, info);
            SerializationHelper.WriteList("AR2:OHE", _onHitEffects, info);
            info.AddValue("AR2:IS", _impactSpeed);
            info.AddValue("AR2:A", _attacker);
            info.AddValue("AR2:T", _target);
            info.AddValue("AR2:M", _medium);
            info.AddValue("AR2:BS", _bypassShield);
            info.AddValue("AR2:BR", _bypassResistance);
            info.AddValue("AR2:E", _evadeable);
            info.AddValue("AR2:P", _periodic);
            info.AddValue("AR2:NP", _noPushback);
            info.AddValue("AR2:PM", _pushbackModifier);
            info.AddValue("AR2:TG", _tg);
            info.AddValue("AR2:S", _s);
            info.AddValue("AR2:EL", _el);
            info.AddValue("AR2:I", _i);
            info.AddValue("AR2:Y", _y);
            info.AddValue("AR2:CM", _cm);
            info.AddValue("AR2:ML", _ml);
            SerializationHelper.WriteList("AR:X", _x, info);
            info.AddValue("AR2:U", _u);
            info.AddValue("AR2:C", _c);
            info.AddValue("AR2:B", _b);
            info.AddValue("AR2:O", _o);
            info.AddValue("AR2:CP", _cp);
            info.AddValue("AR2:SA", _sa);
            info.AddValue("AR2:SB", _sb);
            info.AddValue("AR2:BT", _bt);
            info.AddValue("AR2:RB", _rb);
            info.AddValue("AR2:HV", _hv);
            info.AddValue("AR2:IP", _ip);
            info.AddValue("AR2:NL", _nl);
            info.AddValue("AR2:CW", _cw);
            info.AddValue("AR2:AM", _am);
        }

        public eART _tg;

        public bool _hit;

        public bool? _hv;

        public bool _b;

        public bool _criticalHit;

        public List<Damage> _damages = new List<Damage>();

        public List<HitEffectReference> _onHitEffects = new List<HitEffectReference>();

        public ImpactSpeed _impactSpeed;

        public CharacterInfo _attacker;

        public CharacterInfo _target;

        public string _medium;

        public bool _bypassShield;

        public bool _bt;

        public double _sb;

        public double _rb;

        public bool _bypassResistance;

        public bool _evadeable;

        public bool _periodic;

        public bool _noPushback;

        public double _pushbackModifier = 1.0;

        public bool _s;

        public string _el;

        public bool _i;

        public AmmoType _y;

        public string _cm;

        public bool _ml;

        public bool _u;

        public List<OAIM> _x = new List<OAIM>();

        public object[] _c;

        public PropertyCollection _cp = new PropertyCollection();

        public WeaponSubtype? _o;

        public Guid _sa;

        public bool _ip;

        public bool _nl;

        public WeaponItemInstance _cw;

        public string _am;
    }
}
