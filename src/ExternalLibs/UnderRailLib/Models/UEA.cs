using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Aspects;
using UnderRailLib.TimelapseVertigo.Common;
using UnderRailLib.TimelapseVertigo.Playfield.Space;

namespace UnderRailLib.Models
{
    [EncodedTypeName("UEA")]
    [Serializable]
    public class UEA : Aspect<LE2>, iINA
    {
        protected UEA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            SerializationHelper.ReadList("UEA:AD", ref _ad, info);
            _obad = info.GetBoolean("UEA:OBAD");
            SerializationHelper.ReadList("UEA:OUJ", ref _ouj, info);
            if (DataModelVersion.MinorVersion >= 35)
            {
                _fo = info.GetBoolean("UEA:FO");
                if (DataModelVersion.MinorVersion >= 46)
                {
                    _r = info.GetSingle("UEA:R");
                    _pa = info.GetBoolean("UEA:PA");
                }
                else
                {
                    _pa = true;
                }
            }

            if (DataModelVersion.MinorVersion >= 57)
            {
                _i = (eI) info.GetValue("UEA:I", typeof(eI));
            }

            if (DataModelVersion.MinorVersion >= 70)
            {
                _c = info.GetBoolean("UEA:C");
            }

            if (DataModelVersion.MinorVersion >= 72)
            {
                _u = info.GetBoolean("UEA:U");
            }
            else
            {
                _u = true;
            }

            if (DataModelVersion.MinorVersion >= 74)
            {
                _e = info.GetBoolean("UEA:E");
            }
            else
            {
                _e = true;
            }

            if (DataModelVersion.MinorVersion >= 121)
            {
                _h = info.GetBoolean("UEA:H");
            }

            if (DataModelVersion.MinorVersion >= 126)
            {
                _ch = (info.GetValue("UEA:CH", typeof(TimelapseCursor?)) as TimelapseCursor?);
            }

            if (DataModelVersion.MinorVersion >= 137)
            {
                _up = info.GetBoolean("UEA:UP");
            }
            else
            {
                _up = true;
            }

            if (DataModelVersion.MinorVersion >= 153)
            {
                _a = info.GetBoolean("UEA:A");
            }

            if (DataModelVersion.MinorVersion >= 272)
            {
                _d = info.GetBoolean("UEA:D");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            SerializationHelper.WriteList("UEA:AD", _ad, info);
            info.AddValue("UEA:OBAD", _obad);
            SerializationHelper.WriteList("UEA:OUJ", _ouj, info);
            info.AddValue("UEA:FO", _fo);
            info.AddValue("UEA:R", _r);
            info.AddValue("UEA:PA", _pa);
            info.AddValue("UEA:I", _i);
            info.AddValue("UEA:C", _c);
            info.AddValue("UEA:U", _u);
            info.AddValue("UEA:E", _e);
            info.AddValue("UEA:H", _h);
            info.AddValue("UEA:CH", _ch);
            info.AddValue("UEA:UP", _up);
            info.AddValue("UEA:A", _a);
            info.AddValue("UEA:D", _d);
        }

        private List<HorizontalDirection> _ad;

        private bool _obad;

        private List<Job> _ouj = new List<Job>();

        private bool _fo;

        private bool _c;

        private float _r;

        private bool _pa = true;

        private eI _i;

        private bool _u;

        public bool _e = true;

        public bool _h;

        public TimelapseCursor? _ch;

        private bool _up;

        private bool _a;

        private bool _d;
    }
}
