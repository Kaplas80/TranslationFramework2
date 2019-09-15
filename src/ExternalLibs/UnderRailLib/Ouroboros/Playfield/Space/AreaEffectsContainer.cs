using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models;

namespace UnderRailLib.Ouroboros.Playfield.Space
{
    [EncodedTypeName("AEC")]
    [Serializable]
    public abstract class AreaEffectsContainer<T> : ISerializable where T : AreaEffect
    {
        protected AreaEffectsContainer(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _area = (Area)info.GetValue("AEC:A", typeof(Area));
                _items = (List<T>)info.GetValue("AEC:I", typeof(List<T>));
                return;
            }
            if (GetType() == typeof(AreaEffectsContainer<T>))
            {
                _area = (Area)info.GetValue("_Area", typeof(Area));
                _items = (List<T>)info.GetValue("_Items", typeof(List<T>));
                return;
            }
            _area = (Area)info.GetValue("AreaEffectsContainer`1+_Area", typeof(Area));
            _items = (List<T>)info.GetValue("AreaEffectsContainer`1+_Items", typeof(List<T>));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("AEC:A", _area);
            info.AddValue("AEC:I", _items);
        }

        private Area _area;

        private List<T> _items = new List<T>();
    }
}
