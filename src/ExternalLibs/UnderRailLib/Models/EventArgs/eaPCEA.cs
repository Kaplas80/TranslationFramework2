using System;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models.EventArgs
{
    [EncodedTypeName("eaPCEA")]
    [Serializable]
    public class eaPCEA : System.EventArgs
    {
        public object ObjectA { get; }
        public object ObjectB { get; }
        
        public eaPCEA(object obj1, object obj2)
        {
            ObjectA = obj1;
            ObjectB = obj2;
        }
    }
}
