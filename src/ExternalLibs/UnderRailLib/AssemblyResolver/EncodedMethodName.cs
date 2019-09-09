using System;

namespace UnderRailLib.AssemblyResolver
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class EncodedMethodName : Attribute
    {
        public string GetName()
        {
            return _name;
        }

        public EncodedMethodName(string name)
        {
            _name = name;
        }

        private string _name;
    }
}
