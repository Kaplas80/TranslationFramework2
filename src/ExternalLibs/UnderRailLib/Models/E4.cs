using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Models.EventArgs;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.TimelapseVertigo.Playfield.Space;

namespace UnderRailLib.Models
{
    [EncodedTypeName("E4")]
    [Serializable]
    public class E4 : E2, iPOSE, iIC, iLY, iIAC
    {
        protected E4(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            _layer = info.GetInt32("E4:L");
            _visualPriority = info.GetInt32("E4:VP");
            _location = (Vector3) info.GetValue("E4:L1", typeof(Vector3));
            _size = (Vector3) info.GetValue("E4:S", typeof(Vector3));
            _isFloor = info.GetBoolean("E4:IF");
            _isGround = info.GetBoolean("E4:IG");
            _visualDirection = (HorizontalDirection) info.GetValue("E4:VD", typeof(HorizontalDirection));
            _s1 = (eES) info.GetValue("E4:S1", typeof(eES));
            _mfu = info.GetBoolean("E4:MFU");
            SerializationHelper.ReadList("E4:OT", ref _ot, info);
            _area = (Area) info.GetValue("E4:A", typeof(Area));
            _tileFormationModifiers = (CS) info.GetValue("E4:TFM", typeof(CS));
            SerializationHelper.ReadList("E4:RE", ref _raysEmitters, info);
            _l2 = (Color) info.GetValue("E4:L2", typeof(Color));
            SerializationHelper.ReadDictionary("E4:OHL", ref _ohl, info);
            _o = (SCS) info.GetValue("E4:O", typeof(SCS));
            _d = (CS) info.GetValue("E4:D", typeof(CS));
            _gl = (CS) info.GetValue("E4:GL", typeof(CS));
            _canTrigger = info.GetBoolean("E4:CT");
            if (DataModelVersion.MajorVersion >= 5)
            {
                SerializationHelper.ReadEvent("E4:LC", ref _lc, info);
                SerializationHelper.ReadEvent("E4:SC", ref _sc, info);
                SerializationHelper.ReadEvent("E4:IFC", ref _ifc, info);
                SerializationHelper.ReadEvent("E4:IGC", ref _igc, info);
                SerializationHelper.ReadEvent("E4:D1", ref _d1, info);
                SerializationHelper.ReadEvent("E4:D2", ref _d2, info);
                SerializationHelper.ReadEvent("E4:U", ref _u, info);
                SerializationHelper.ReadEvent("E4:U1", ref _u1, info);
                SerializationHelper.ReadEvent("E4:D3", ref _d3, info);
                SerializationHelper.ReadEvent("E4:D4", ref _d4, info);
                SerializationHelper.ReadEvent("E4:VDC", ref _vdc, info);
                if (DataModelVersion.MajorVersion >= 18)
                {
                    _f8 = (info.GetValue("E4:F8", typeof(Vector2?)) as Vector2?);
                    _f9 = info.GetBoolean("E4:F9");
                }

                if (DataModelVersion.MajorVersion >= 30)
                {
                    _ao = info.GetBoolean("E4:AO");
                }

                if (DataModelVersion.MajorVersion >= 31)
                {
                    _designOnlyEditable = info.GetBoolean("E4:X");
                }
            }
            else
            {
                _lc = (EventHandler<eaPCEA>) info.GetValue("E4:LC", typeof(EventHandler<eaPCEA>));
                _sc = (EventHandler<eaPCEA>) info.GetValue("E4:SC", typeof(EventHandler<eaPCEA>));
                _ifc = (EventHandler<eaPCEA>) info.GetValue("E4:IFC", typeof(EventHandler<eaPCEA>));
                _igc = (EventHandler<eaPCEA>) info.GetValue("E4:IGC", typeof(EventHandler<eaPCEA>));
                _d1 = (EventHandler<System.EventArgs>) info.GetValue("E4:D1",
                    typeof(EventHandler<System.EventArgs>));
                _d2 = (EventHandler<System.EventArgs>) info.GetValue("E4:D2",
                    typeof(EventHandler<System.EventArgs>));
                _u = (EventHandler<System.EventArgs>) info.GetValue("E4:U", typeof(EventHandler<System.EventArgs>));
                _u1 = (EventHandler<System.EventArgs>) info.GetValue("E4:U1",
                    typeof(EventHandler<System.EventArgs>));
                _d3 = (EventHandler<System.EventArgs>) info.GetValue("E4:D3",
                    typeof(EventHandler<System.EventArgs>));
                _d4 = (EventHandler<System.EventArgs>) info.GetValue("E4:D4",
                    typeof(EventHandler<System.EventArgs>));
                _vdc = (EventHandler<eaPCEA>) info.GetValue("E4:VDC", typeof(EventHandler<eaPCEA>));
            }

            if (DataModelVersion.MajorVersion >= 2)
            {
                SerializationHelper.ReadDataModelTypeKeyedDictionary("E4:A1", ref v, info);
                return;
            }

            SerializationHelper.ReadDictionary("E4:A1", ref v, info);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("E4:L", _layer);
            info.AddValue("E4:L1", _location);
            info.AddValue("E4:S", _size);
            info.AddValue("E4:IF", _isFloor);
            info.AddValue("E4:IG", _isGround);
            info.AddValue("E4:VP", _visualPriority);
            info.AddValue("E4:VD", _visualDirection);
            info.AddValue("E4:S1", _s1);
            info.AddValue("E4:MFU", _mfu);
            SerializationHelper.WriteList("E4:OT", _ot, info);
            info.AddValue("E4:A", _area);
            info.AddValue("E4:TFM", _tileFormationModifiers);
            SerializationHelper.WriteList("E4:RE", _raysEmitters, info);
            info.AddValue("E4:L2", _l2);
            SerializationHelper.WriteDictionary("E4:OHL", _ohl, info);
            info.AddValue("E4:O", _o);
            info.AddValue("E4:D", _d);
            info.AddValue("E4:GL", _gl);
            info.AddValue("E4:CT", _canTrigger);
            info.AddValue("E4:F8", _f8);
            info.AddValue("E4:F9", _f9);
            info.AddValue("E4:AO", _ao);
            SerializationHelper.WriteEvent("E4:LC", _lc, info);
            SerializationHelper.WriteEvent("E4:SC", _sc, info);
            SerializationHelper.WriteEvent("E4:IFC", _ifc, info);
            SerializationHelper.WriteEvent("E4:IGC", _igc, info);
            SerializationHelper.WriteEvent("E4:D1", _d1, info);
            SerializationHelper.WriteEvent("E4:D2", _d2, info);
            SerializationHelper.WriteEvent("E4:U", _u, info);
            SerializationHelper.WriteEvent("E4:U1", _u1, info);
            SerializationHelper.WriteEvent("E4:D3", _d3, info);
            SerializationHelper.WriteEvent("E4:D4", _d4, info);
            SerializationHelper.WriteEvent("E4:VDC", _vdc, info);
            SerializationHelper.WriteDataModelTypeKeyedDictionary("E4:A1", v, info);
            info.AddValue("E4:X", _designOnlyEditable);
        }

        private int _layer;

        public Vector3 _location;

        public Vector3 _size;

        public bool _isFloor;

        public bool _isGround;

        public int _visualPriority;

        private HorizontalDirection _visualDirection;

        public eES _s1;

        private bool _mfu;

        public List<T1> _ot = new List<T1>();

        internal Area _area;

        private CS _tileFormationModifiers;

        private List<RaysEmitter> _raysEmitters = new List<RaysEmitter>();

        private Color _l2;

        public Dictionary<byte, Color?> _ohl = new Dictionary<byte, Color?>();

        private SCS _o;

        internal CS _d;

        internal CS _gl;

        private Dictionary<Type, A> v = new Dictionary<Type, A>();

        private bool _canTrigger;

        public bool _f9;

        public bool _ao;

        public Vector2? _f8;

        public bool _designOnlyEditable;

        private EventHandler<eaPCEA> _lc;

        private EventHandler<eaPCEA> _sc;

        private EventHandler<eaPCEA> _ifc;

        private EventHandler<eaPCEA> _igc;

        private EventHandler<System.EventArgs> _d1;

        private EventHandler<System.EventArgs> _d2;

        private EventHandler<System.EventArgs> _u;

        private EventHandler<System.EventArgs> _u1;

        private EventHandler<System.EventArgs> _d3;

        private EventHandler<System.EventArgs> _d4;

        private EventHandler<eaPCEA> _vdc;
    }
}
