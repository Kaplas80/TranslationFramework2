using System;

namespace UnderRailLib.AssemblyResolver
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false, AllowMultiple = false)]
    public sealed class EncodedTypeName : Attribute
    {
        public string GetName()
        {
            return _type;
        }

        public EncodedTypeName(string type)
        {
            _type = type;
        }

        private readonly string _type;
    }
}
