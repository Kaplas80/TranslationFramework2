using System;
using System.Collections.Generic;

namespace TF.Core.Helpers
{
    public static class TypeHelper
    {
        private static readonly Dictionary<string, Type> TypesDictionary = new Dictionary<string, Type>();

        public static Type GetType(string name)
        {
            if (TypesDictionary.ContainsKey(name))
            {
                return TypesDictionary[name];
            }

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in a.GetTypes())
                {
                    if (t.FullName == name)
                    {
                        TypesDictionary.Add(name, t);
                        return t;
                    }
                }
            }

            return null;
        }
    }
}
