using System;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models.EventArgs
{
    [EncodedTypeName("eaNCEA")]
    [Serializable]
    public class eaNCEA : System.EventArgs
    {
        public Point Point { get; }
        
        public eaNCEA(Point point)
        {
            Point = point;
        }
    }
}
