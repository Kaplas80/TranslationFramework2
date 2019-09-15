using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("F")]
    [Serializable]
    public class Feats : iCHP, ISerializable
    {
        protected Feats(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _character = (C1)info.GetValue("F:C", typeof(C1));
                SerializationHelper.ReadList("F:F", ref _feats, info);
                if (DataModelVersion.MajorVersion >= 5)
                {
                    SerializationHelper.ReadEvent("F:FA", ref _featAdded, info);
                    SerializationHelper.ReadEvent("F:FR", ref _featRemoved, info);
                    return;
                }
                _featAdded = (EventHandler<System.EventArgs>)info.GetValue("F:FA", typeof(EventHandler<System.EventArgs>));
                _featRemoved = (EventHandler<System.EventArgs>)info.GetValue("F:FR", typeof(EventHandler<System.EventArgs>));
            }
            else
            {
                if (GetType() == typeof(Feats))
                {
                    _character = (C1)info.GetValue("_Character", typeof(C1));
                    _feats = (List<FeatReference>)info.GetValue("_Feats", typeof(List<FeatReference>));
                    _featAdded = (EventHandler<System.EventArgs>)info.GetValue("FeatAdded", typeof(EventHandler<System.EventArgs>));
                    _featRemoved = (EventHandler<System.EventArgs>)info.GetValue("FeatRemoved", typeof(EventHandler<System.EventArgs>));
                    return;
                }
                _character = (C1)info.GetValue("Feats+_Character", typeof(C1));
                _feats = (List<FeatReference>)info.GetValue("Feats+_Feats", typeof(List<FeatReference>));
                _featAdded = (EventHandler<System.EventArgs>)info.GetValue("Feats+FeatAdded", typeof(EventHandler<System.EventArgs>));
                _featRemoved = (EventHandler<System.EventArgs>)info.GetValue("Feats+FeatRemoved", typeof(EventHandler<System.EventArgs>));
            }
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("F:C", _character);
            SerializationHelper.WriteList("F:F", _feats, info);
            SerializationHelper.WriteEvent("F:FA", _featAdded, info);
            SerializationHelper.WriteEvent("F:FR", _featRemoved, info);
        }

        private C1 _character;

        private List<FeatReference> _feats = new List<FeatReference>();

        private EventHandler<System.EventArgs> _featAdded;

        private EventHandler<System.EventArgs> _featRemoved;
    }
}
