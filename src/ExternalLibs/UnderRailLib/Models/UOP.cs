using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("UOP")]
    [Serializable]
    public sealed class UOP : Job
    {
        private UOP(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _i = (Guid?) info.GetValue("UOP:I", typeof(Guid?));
            _a = info.GetString("UOP:A");
            _r = info.GetBoolean("UOP:R");
            _co = info.GetBoolean("UOP:CO");
            _cof = info.GetBoolean("UOP:COF");
            _cp = info.GetBoolean("UOP:CP");
            _cpf = info.GetBoolean("UOP:CPF");
            _ota = (info.GetValue("UOP:OTA", typeof(List<string>)) as List<string>);
            _otr = (info.GetValue("UOP:OTR", typeof(List<string>)) as List<string>);
            _ofta = (info.GetValue("UOP:OFTA", typeof(List<string>)) as List<string>);
            _oftr = (info.GetValue("UOP:OFTR", typeof(List<string>)) as List<string>);
            _pta = (info.GetValue("UOP:PTA", typeof(List<string>)) as List<string>);
            _ptr = (info.GetValue("UOP:PTR", typeof(List<string>)) as List<string>);
            _pfta = (info.GetValue("UOP:PFTA", typeof(List<string>)) as List<string>);
            _pftr = (info.GetValue("UOP:PFTR", typeof(List<string>)) as List<string>);
            _bm = (info.GetValue("UOP:BM", typeof(eFBCM?)) as eFBCM?);
            _pe = (info.GetValue("UOP:PE", typeof(bool?)) as bool?);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("UOP:I", _i);
            info.AddValue("UOP:A", _a);
            info.AddValue("UOP:R", _r);
            info.AddValue("UOP:CO", _co);
            info.AddValue("UOP:COF", _cof);
            info.AddValue("UOP:CP", _cp);
            info.AddValue("UOP:CPF", _cpf);
            info.AddValue("UOP:OTA", _ota);
            info.AddValue("UOP:OTR", _otr);
            info.AddValue("UOP:OFTA", _ofta);
            info.AddValue("UOP:OFTR", _oftr);
            info.AddValue("UOP:PTA", _pta);
            info.AddValue("UOP:PTR", _ptr);
            info.AddValue("UOP:PFTA", _pfta);
            info.AddValue("UOP:PFTR", _pftr);
            info.AddValue("UOP:BM", _bm);
            info.AddValue("UOP:PE", _pe);
        }

        private Guid? _i;

        private string _a;

        private bool _r;

        private bool _co;

        private bool _cof;

        private bool _cp;

        private bool _cpf;

        private List<string> _ota = new List<string>();

        private List<string> _otr = new List<string>();

        private List<string> _ofta = new List<string>();

        private List<string> _oftr = new List<string>();

        private List<string> _pta = new List<string>();

        private List<string> _ptr = new List<string>();

        private List<string> _pfta = new List<string>();

        private List<string> _pftr = new List<string>();

        private eFBCM? _bm;

        private bool? _pe = true;
    }
}
