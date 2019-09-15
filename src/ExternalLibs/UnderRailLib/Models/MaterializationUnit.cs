using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("MU")]
    [Serializable]
    public sealed class MaterializationUnit : ISerializable
    {
        private MaterializationUnit(SerializationInfo info, StreamingContext ctx)
        {
            _n = info.GetString("MU:N");
            _t = info.GetString("MU:T");
            _s = (Point)info.GetValue("MU:S", typeof(Point));
            _p = info.GetBoolean("MU:P");
            _uo = (Point)info.GetValue("MU:UO", typeof(Point));
            _to = (Vector2)info.GetValue("MU:TO", typeof(Vector2));
            _ms = (Point)info.GetValue("MU:MS", typeof(Point));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("MU:N", _n);
            info.AddValue("MU:T", _t);
            info.AddValue("MU:S", _s);
            info.AddValue("MU:P", _p);
            info.AddValue("MU:UO", _uo);
            info.AddValue("MU:TO", _to);
            info.AddValue("MU:MS", _ms);
        }

        public string _n;

        public string _t;

        public Point _s;

        public bool _p;

        public Point _uo;

        public Vector2 _to;

        public Point _ms;
    }
}
