using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.EventArgs;

namespace UnderRailLib.Models
{
    [EncodedTypeName("T1")]
    [Serializable]
    public class T1 : ISerializable
    {
        protected T1(SerializationInfo info, StreamingContext ctx)
        {
            _entities = (SortedList<E4, E4>) info.GetValue("T1:E", typeof(SortedList<E4, E4>));
            SerializationHelper.ReadDictionary("T1:LS", ref _ls, info);
            _liv = info.GetBoolean("T1:LIV");
            _cl = (Color) info.GetValue("T1:CL", typeof(Color));
            _o = (SCS) info.GetValue("T1:O", typeof(SCS));
            _d = (CS) info.GetValue("T1:D", typeof(CS));
            SerializationHelper.ReadDictionary("T1:EFM", ref _efm, info);
            _fiv = info.GetBoolean("T1:FIV");
            _cf = (CS) info.GetValue("T1:CF", typeof(CS));
            if (DataModelVersion.MajorVersion >= 5)
            {
                SerializationHelper.ReadEvent("T1:FC", ref _fc, info);
                SerializationHelper.ReadEvent("T1:LCC", ref _lcc, info);
                SerializationHelper.ReadEvent("T1:LC", ref _lc, info);
            }
            else
            {
                _fc = (EventHandler<eaNCEA>) info.GetValue("T1:FC", typeof(EventHandler<eaNCEA>));
                _lcc = (EventHandler<eaNCEA>) info.GetValue("T1:LCC", typeof(EventHandler<eaNCEA>));
                _lc = (EventHandler<System.EventArgs>) info.GetValue("T1:LC", typeof(EventHandler<System.EventArgs>));
            }

            if (DataModelVersion.MajorVersion >= 16)
            {
                _fo = (info.GetValue("T1:FO", typeof(Dictionary<Guid, CS>)) as Dictionary<Guid, CS>);
                return;
            }

            _fo = new Dictionary<Guid, CS>();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("T1:E", _entities);
            SerializationHelper.WriteDictionary("T1:LS", _ls, info);
            info.AddValue("T1:LIV", _liv);
            info.AddValue("T1:CL", _cl);
            info.AddValue("T1:O", _o);
            info.AddValue("T1:D", _d);
            SerializationHelper.WriteDictionary("T1:EFM", _efm, info);
            info.AddValue("T1:FIV", _fiv);
            info.AddValue("T1:CF", _cf);
            info.AddValue("T1:FO", _fo);
            SerializationHelper.WriteEvent("T1:FC", _fc, info);
            SerializationHelper.WriteEvent("T1:LCC", _lcc, info);
            SerializationHelper.WriteEvent("T1:LC", _lc, info);
        }

        public SortedList<E4, E4> _entities;

        private Dictionary<Guid, LightStamp> _ls;

        private bool _liv;

        private Color _cl;

        private SCS _o;

        internal CS _d;

        private Dictionary<Guid, long> _efm;

        private bool _fiv;

        private CS _cf;

        public Dictionary<Guid, CS> _fo;

        private EventHandler<eaNCEA> _fc;

        private EventHandler<eaNCEA> _lcc;

        private EventHandler<System.EventArgs> _lc;
    }
}
