using System;
using System.Runtime.Serialization;

namespace UnderRailLib.AssemblyResolver
{
    public sealed class Binder : SerializationBinder
    {
        private Binder()
        {
        }

        public static Binder Instance => _instance ?? (_instance = new Binder());

        public static void SetAssemblyResolver(AssemblyResolver resolver)
        {
            Resolver = resolver;
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            if (Resolver == null)
            {
                throw new InvalidOperationException("Assembly resolver not set!");
            }
            
            return Resolver.GetType(typeName, assemblyName);
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            if (Resolver == null)
            {
                throw new InvalidOperationException("Assembly resolver not set!");
            }

            Resolver.GetName(serializedType, out typeName, out assemblyName, true);
        }

        public static AssemblyResolver Resolver;

        private static Binder _instance;
    }
}
