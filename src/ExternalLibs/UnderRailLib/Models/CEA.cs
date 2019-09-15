using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common;
using UnderRailLib.Ouroboros.Common.Aspects;
using UnderRailLib.TimelapseVertigo.Playfield.Locale;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CEA")]
    [Serializable]
    public sealed class CEA : Aspect<LE2>, iMVH, iMOB, iINA, iSHOP, iSA, iTBCEA, iLSTA
    {
        private CEA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _model = info.GetString("CEA:M");
                _player = info.GetBoolean("CEA:P");
                _visibleInFog = info.GetBoolean("CEA:VIF");
                _character = (IProvider<C1>) info.GetValue("CEA:C", typeof(IProvider<C1>));
                _specialStates = (CharacterSpecialStates) info.GetValue("CEA:SS", typeof(CharacterSpecialStates));
                _lootbagModel = info.GetString("CEA:LM");
                _alwaysSpawnRemains = info.GetBoolean("CEA:ASR");
                _supressMaterializer = info.GetBoolean("CEA:SM");
                SerializationHelper.ReadList("CEA:CIAJ", ref _characterIsAttackedJobs, info);
                SerializationHelper.ReadList("CEA:CDJ", ref _characterDiedJobs, info);
                _searchTimer = info.GetDouble("CEA:ST");
                _customXpAmount = (int?) info.GetValue("CEA:CXA", typeof(int?));
                _xpModifier = info.GetSingle("CEA:XM");
                if (DataModelVersion.MinorVersion >= 11)
                {
                    _snd = (info.GetValue("CEA:SND", typeof(CSN)) as CSN);
                }

                if (DataModelVersion.MinorVersion >= 25)
                {
                    _cmc = (eCMC) info.GetValue("CEA:CMC", typeof(eCMC));
                }
                else
                {
                    _cmc = eCMC.a;
                }

                if (DataModelVersion.MinorVersion >= 59)
                {
                    _h = info.GetSingle("CEA:H");
                    _g = info.GetSingle("CEA:G");
                }
                else
                {
                    _h = 1f;
                    _g = 1f;
                }

                if (DataModelVersion.MinorVersion >= 84)
                {
                    _c1 = (info.GetValue("CEA:C1", typeof(CharacterState?)) as CharacterState?);
                    _c2 = (info.GetValue("CEA:C2", typeof(int?)) as int?);
                }

                if (DataModelVersion.MinorVersion >= 85)
                {
                    _lc = info.GetBoolean("CEA:LC");
                }

                if (DataModelVersion.MinorVersion >= 186)
                {
                    _c3 = (info.GetValue("CEA:C3", typeof(CharacterState?)) as CharacterState?);
                    _c4 = (info.GetValue("CEA:C4", typeof(int?)) as int?);
                }

                if (DataModelVersion.MinorVersion >= 201)
                {
                    _sl = info.GetString("CEA:SL");
                }

                if (DataModelVersion.MinorVersion >= 282)
                {
                    _pst = info.GetDouble("CEA:PST");
                }

                if (DataModelVersion.MinorVersion >= 310)
                {
                    _ader = info.GetBoolean("CEA:ADER");
                }

                if (DataModelVersion.MinorVersion >= 315)
                {
                    _pu = info.GetBoolean("CEA:PU");
                }

                if (DataModelVersion.MinorVersion >= 383)
                {
                    _vc = (info.GetValue("CEA:VC", typeof(CVHC)) as CVHC);
                }

                if (DataModelVersion.MinorVersion >= 384)
                {
                    _vcd = info.GetBoolean("CEA:VCD");
                }

                if (DataModelVersion.MinorVersion >= 401)
                {
                    _ivc = (info.GetValue("CEA:IVC", typeof(IVC)) as IVC);
                }

                if (DataModelVersion.MinorVersion >= 409)
                {
                    SerializationHelper.ReadList("CEA:PSJ", ref _psj, info);
                }
                else
                {
                    _psj = new List<Job>();
                }

                if (DataModelVersion.MinorVersion >= 434)
                {
                    _mec = info.GetString("CEA:MEC");
                    _mecd = info.GetString("CEA:MECD");
                }

                if (DataModelVersion.MinorVersion >= 456)
                {
                    SerializationHelper.ReadList("CEA:HBJ", ref _hbj, info);
                }
                else
                {
                    _hbj = new List<Job>();
                }

                if (DataModelVersion.MinorVersion >= 498)
                {
                    SerializationHelper.ReadList("CEA:CDF", ref cdf, info);
                }
                else
                {
                    cdf = new List<Job>();
                }

                if (DataModelVersion.MinorVersion >= 522)
                {
                    SerializationHelper.ReadList("CEA:CMJ", ref _cmj, info);
                }
                else
                {
                    _cmj = new List<Job>();
                }

                if (DataModelVersion.MinorVersion >= 523)
                {
                    _p2 = (info.GetValue("CEA:P2", typeof(int?)) as int?);
                }
            }
            else
            {
                _player = info.GetBoolean("_Player");
                _alwaysSpawnRemains = info.GetBoolean("_AlwaysSpawnRemains");
                _visibleInFog = info.GetBoolean("_VisibleInFog");
                _searchTimer = info.GetDouble("_SearchTimer");
                _customXpAmount = (info.GetValue("_CustomXpAmount", typeof(int?)) as int?);
                _xpModifier = info.GetSingle("_XpModifier");
                _model = info.GetString("_Model");
                _character = (info.GetValue("_Character", typeof(IProvider<C1>)) as IProvider<C1>);
                _specialStates = (info.GetValue("_SpecialStates", typeof(CharacterSpecialStates)) as CharacterSpecialStates);
                _lootbagModel = info.GetString("_LootbagModel");
                _supressMaterializer = info.GetBoolean("_SuppressMaterializer");
                SerializationHelper.ReadList("_CharacterIsAttackedJobs", ref _characterIsAttackedJobs, info);
                SerializationHelper.ReadList("_CharacterDiedJobs", ref _characterDiedJobs, info);
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CEA:M", _model);
            info.AddValue("CEA:P", _player);
            info.AddValue("CEA:VIF", _visibleInFog);
            info.AddValue("CEA:C", _character);
            info.AddValue("CEA:SS", _specialStates);
            info.AddValue("CEA:LM", _lootbagModel);
            info.AddValue("CEA:ASR", _alwaysSpawnRemains);
            info.AddValue("CEA:SM", _supressMaterializer);
            SerializationHelper.WriteList("CEA:CIAJ", _characterIsAttackedJobs, info);
            SerializationHelper.WriteList("CEA:CDJ", _characterDiedJobs, info);
            info.AddValue("CEA:ST", _searchTimer);
            info.AddValue("CEA:CXA", _customXpAmount);
            info.AddValue("CEA:XM", _xpModifier);
            info.AddValue("CEA:SND", _snd);
            info.AddValue("CEA:CMC", _cmc);
            info.AddValue("CEA:H", _h);
            info.AddValue("CEA:G", _g);
            info.AddValue("CEA:C1", _c1);
            info.AddValue("CEA:C2", _c2);
            info.AddValue("CEA:P2", _p2);
            info.AddValue("CEA:C3", _c3);
            info.AddValue("CEA:C4", _c4);
            info.AddValue("CEA:LC", _lc);
            info.AddValue("CEA:SL", _sl);
            info.AddValue("CEA:PST", _pst);
            info.AddValue("CEA:ADER", _ader);
            info.AddValue("CEA:PU", _pu);
            info.AddValue("CEA:VC", _vc);
            info.AddValue("CEA:VCD", _vcd);
            info.AddValue("CEA:IVC", _ivc);
            SerializationHelper.WriteList("CEA:PSJ", _psj, info);
            info.AddValue("CEA:MEC", _mec);
            info.AddValue("CEA:MECD", _mecd);
            SerializationHelper.WriteList("CEA:HBJ", _hbj, info);
            SerializationHelper.WriteList("CEA:CDF", cdf, info);
            SerializationHelper.WriteList("CEA:CMJ", _cmj, info);
        }

        private string _model;

        private bool _player;

        private bool _visibleInFog;

        private IProvider<C1> _character;

        private CharacterSpecialStates _specialStates;

        private string _lootbagModel = "default";

        private bool _alwaysSpawnRemains;

        private bool _ader;

        private bool _lc;

        private bool _supressMaterializer;

        private bool _pu;

        private List<Job> _characterIsAttackedJobs = new List<Job>();

        private List<Job> _characterDiedJobs = new List<Job>();

        private List<Job> _psj = new List<Job>();

        private List<Job> _hbj = new List<Job>();

        private List<Job> cdf = new List<Job>();

        private List<Job> _cmj = new List<Job>();

        private double _searchTimer;

        private double _pst;

        private int? _customXpAmount;

        private float _xpModifier;

        private CSN _snd;

        private eCMC _cmc;

        public float _h = 1f;

        public float _g = 1f;

        public CharacterState? _c1;

        public int? _c2;

        public int? _p2;

        public CharacterState? _c3;

        public int? _c4;

        private string _sl;

        private CVHC _vc;

        private bool _vcd;

        private IVC _ivc;

        private string _mec;

        private string _mecd;
    }
}
