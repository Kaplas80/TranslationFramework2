using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace UnderRailLib.AssemblyResolver
{
    public class AssemblyResolver
    {
        public Type GetType(string typeName, string assemblyName, bool throwExceptions = true)
        {
            Type type = null;
            switch (assemblyName)
            {
                case "X":
                    _netEncodedTypes.TryGetValue(typeName, out type);
                    break;
                case "G":
                {
                    _customEncodedTypes.TryGetValue(typeName, out type);
                    if (type == null)
                    {
                        type = DecodeType(typeName);
                        _customEncodedTypes[typeName] = type;
                        return type;
                    }

                    break;
                }
            }

            if (type != null) 
            {
                return type;
            }

            type = GetUncodedType(typeName, assemblyName);
            if (type == null && throwExceptions)
            {
                throw new Exception("Unable to resolve data element type: " + typeName);
            }

            _customEncodedTypes[typeName] = type;

            return type;
        }

        public string GetMethodName(MethodInfo methodInfo)
        {
            _methodInfoToMethodName.TryGetValue(methodInfo, out var result);
            return result;
        }

        public MethodInfo GetMethodInfo(string methodName)
        {
            _methodNameToMethodInfo.TryGetValue(methodName, out var result);
            return result;
        }

        public void GetName(Type serializedType, out string typeName, out string assemblyName, bool A_3 = false)
        {
            _typeEncodedName.TryGetValue(serializedType, out typeName);
            if (typeName != null)
            {
                assemblyName = (serializedType.IsGenericType ? "G" : "X");
                return;
            }

            if (A_3 && typeof(Delegate).IsAssignableFrom(serializedType))
            {
                typeName = null;
                assemblyName = null;
                return;
            }

            if (serializedType.IsGenericType || serializedType.IsArray)
            {
                typeName = GetEncodedTypeName(serializedType);
                assemblyName = "G";
                _typeEncodedName[serializedType] = typeName;
                return;
            }

            _typeNameMapping.TryGetValue(serializedType.FullName, out typeName);
            if (typeName != null)
            {
                assemblyName = "X";
                _typeEncodedName[serializedType] = typeName;
                return;
            }

            if (A_3)
            {
                typeName = null;
                assemblyName = null;
                return;
            }

            typeName = serializedType.FullName;
            assemblyName = serializedType.Assembly.FullName;
        }

        private string GetTypeName(Type type)
        {
            _typeNameMapping.TryGetValue(type.FullName, out var assemblyQualifiedName);
            return assemblyQualifiedName ?? (type.AssemblyQualifiedName);
        }

        private string GetEncodedTypeName(Type type)
        {
            string text;
            if (type.IsGenericType)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();
                var arg = GetTypeName(genericTypeDefinition);
                var genericArguments = type.GetGenericArguments();
                text = arg + "#G" + genericArguments.Length;
                text = genericArguments.Aggregate(text, (current, a_) => current + ">" + GetEncodedTypeName(a_));
            }
            else if (type.IsArray)
            {
                text = string.Concat("%A%#", type.GetArrayRank(), ">", GetEncodedTypeName(type.GetElementType()));
            }
            else
            {
                text = GetTypeName(type) + "#-";
            }

            return text;
        }

        public void Initialize()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                _typeList.Add(type);
            }

            MapNetTypes();

            foreach (var type in _typeList)
            {
                _uncodedTypes[type.FullName] = type;
                var encodedType = GetEncodedName(type);
                if (encodedType == null || string.IsNullOrWhiteSpace(encodedType.GetName()))
                {
                    continue;
                }

                if (!AlreadyInserted(encodedType.GetName(), type))
                {
                    MapTypeMethods(type, encodedType.GetName());
                }
            }
        }

        private static EncodedTypeName GetEncodedName(MemberInfo memberInfo)
        {
            return memberInfo.GetCustomAttributes(typeof(EncodedTypeName), false).FirstOrDefault() as EncodedTypeName;
        }

        private static EncodedMethodName GetEncodedName(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttributes(typeof(EncodedMethodName), true).FirstOrDefault() as EncodedMethodName;
        }

        private void MapNetTypes()
        {
            _netEncodedTypes["%b%"] = typeof(bool);
            _netEncodedTypes["%i%"] = typeof(int);
            _netEncodedTypes["%l%"] = typeof(long);
            _netEncodedTypes["%f%"] = typeof(float);
            _netEncodedTypes["%d%"] = typeof(double);
            _netEncodedTypes["%B%"] = typeof(byte);
            _netEncodedTypes["%c%"] = typeof(char);
            _netEncodedTypes["%s%"] = typeof(string);
            _netEncodedTypes["%D%"] = typeof(decimal);
            _netEncodedTypes["%G%"] = typeof(Guid);
            _netEncodedTypes["%NL%"] = typeof(Nullable<>);
            _netEncodedTypes["%DI%"] = typeof(Dictionary<,>);
            _netEncodedTypes["%LI%"] = typeof(List<>);
            _netEncodedTypes["%HS%"] = typeof(HashSet<>);
            _netEncodedTypes["%HT%"] = typeof(Hashtable);
            _netEncodedTypes["%SL%"] = typeof(SortedList<,>);
            _netEncodedTypes["%CO%"] = typeof(Collection<>);

            _typeNameMapping[typeof(bool).FullName] = "%b%";
            _typeNameMapping[typeof(int).FullName] = "%i%";
            _typeNameMapping[typeof(long).FullName] = "%l%";
            _typeNameMapping[typeof(float).FullName] = "%f%";
            _typeNameMapping[typeof(double).FullName] = "%d%";
            _typeNameMapping[typeof(byte).FullName] = "%B%";
            _typeNameMapping[typeof(char).FullName] = "%c%";
            _typeNameMapping[typeof(string).FullName] = "%s%";
            _typeNameMapping[typeof(decimal).FullName] = "%D%";
            _typeNameMapping[typeof(Guid).FullName] = "%G%";
            _typeNameMapping[typeof(Nullable<>).FullName] = "%NL%";
            _typeNameMapping[typeof(Dictionary<,>).FullName] = "%DI%";
            _typeNameMapping[typeof(List<>).FullName] = "%LI%";
            _typeNameMapping[typeof(HashSet<>).FullName] = "%HS%";
            _typeNameMapping[typeof(Hashtable).FullName] = "%HT%";
            _typeNameMapping[typeof(SortedList<,>).FullName] = "%SL%";
            _typeNameMapping[typeof(Collection<>).FullName] = "%CO%";
        }

        private Type GetUncodedType(string typeName, string assemblyName)
        {
            if (typeName.StartsWith("Ouroboros") || typeName.StartsWith("Timelapse") || assemblyName.StartsWith("Ouroboros") || assemblyName.StartsWith("Timelapse"))
            {
                var message = "Resolving uncoded normal type: " + typeName + ", " + assemblyName;
                Trace.WriteLine(message);
            }

            Type type = null;
            try
            {
                _uncodedTypes.TryGetValue(typeName, out type);
                if (type != null)
                {
                    return type;
                }

                type = Type.GetType(typeName);
                if (type != null)
                {
                    return type;
                }

                if (assemblyName != null && assemblyName.Length > 1)
                {
                    type = Type.GetType(typeName + ", " + assemblyName);
                    if (type != null)
                    {
                        return type;
                    }
                }

                if (typeName.Contains(','))
                {
                    type = Type.GetType(typeName.Split(',')[0]);
                    return type;
                }
            }
            finally
            {
                if (type != null)
                {
                    _uncodedTypes[typeName] = type;
                }
            }

            return null;
        }

        private Type DecodeType(string typeName)
        {
            if (typeName == null)
            {
                throw new Exception("Null data model symbol encountered.");
            }

            var split = typeName.Split('>');
            var num = 0;
            return DecodeDataModelSymbol(split, ref num, 1)[0];
        }

        private Type[] DecodeDataModelSymbol(string[] A_0, ref int A_1, int A_2)
        {
            var list = new List<Type>();
            for (var i = 0; i < A_2; i++)
            {
                var text = A_0[A_1];
                var array = text.Split('#');
                if (array.Length < 1)
                {
                    throw new Exception("Cannot decode data model symbol '" + text + "'");
                }

                if (array.Length <= 1)
                {
                    throw new Exception("Cannot decode data model symbol '" + text + "'. Missing components.");
                }

                var a_ = array[0];
                var flag = a_ == "%A%";
                int num;
                bool flag2;
                if (flag)
                {
                    if (!int.TryParse(array[1], NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                    {
                        num = 0;
                    }

                    flag2 = false;
                }
                else
                {
                    var c = array[1].Length >= 1 ? array[1][0] : '-';

                    if (c == 'G')
                    {
                        flag2 = true;
                        if (!int.TryParse(array[1].Substring(1), NumberStyles.Number, CultureInfo.InvariantCulture, out num))
                        {
                            num = 0;
                        }
                    }
                    else
                    {
                        flag2 = false;
                        num = 0;
                    }
                }

                if (flag)
                {
                    if (num <= 0)
                    {
                        throw new Exception("Cannot decode data model symbol '" + text + "'. Invalid array rank.");
                    }

                    if (A_1 + 1 >= A_0.Length)
                    {
                        throw new Exception("Cannot decode data model symbol '" + text + "'. Unexpected end of sequence.");
                    }

                    A_1++;
                    var type = DecodeDataModelSymbol(A_0, ref A_1, 1)[0];
                    var item = num > 1 ? type.MakeArrayType(num) : type.MakeArrayType();

                    list.Add(item);
                }
                else if (flag2 && num > 0)
                {
                    var type2 = GetType(a_, "X");
                    if (A_1 + num >= A_0.Length)
                    {
                        throw new Exception("Cannot decode data model symbol '" + text + "'. Unexpected end of sequence.");
                    }

                    A_1++;
                    var typeArguments = DecodeDataModelSymbol(A_0, ref A_1, num);
                    var item2 = type2.MakeGenericType(typeArguments);
                    list.Add(item2);
                }
                else
                {
                    list.Add(GetType(a_, "X"));
                    A_1++;
                }
            }

            return list.ToArray();
        }

        private bool AlreadyInserted(string encodedType, Type type)
        {
            if (_typeNameMapping.ContainsKey(type.FullName))
            {
                return true;
            }

            _typeNameMapping[type.FullName] = encodedType;
            if (_netEncodedTypes.ContainsKey(encodedType))
            {
                return true;
            }

            _netEncodedTypes[encodedType] = type;
            return false;
        }

        private void MapTypeMethods(Type type, string encodedTypeName)
        {
            foreach (var methodInfo in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance |
                                                       BindingFlags.Static | BindingFlags.Public |
                                                       BindingFlags.NonPublic))
            {
                var encodedMethodName = GetEncodedName(methodInfo);
                if (encodedMethodName == null || string.IsNullOrEmpty(encodedMethodName.GetName())) 
                {
                    continue;
                }

                var text = encodedTypeName + ":" + encodedMethodName.GetName();
                _methodInfoToMethodName[methodInfo] = text;
                if (!_methodNameToMethodInfo.ContainsKey(text))
                {
                    _methodNameToMethodInfo.Add(text, methodInfo);
                }
            }
        }

        private List<Type> _typeList = new List<Type>();
        private Dictionary<string, Type> _netEncodedTypes = new Dictionary<string, Type>(); //_g
        private Dictionary<string, Type> _customEncodedTypes = new Dictionary<string, Type>();
        private Dictionary<string, Type> _uncodedTypes = new Dictionary<string, Type>();
        private Dictionary<string, string> _typeNameMapping = new Dictionary<string, string>(); //_h
        private Dictionary<MethodInfo, string> _methodInfoToMethodName = new Dictionary<MethodInfo, string>(); //_k
        private Dictionary<string, MethodInfo> _methodNameToMethodInfo = new Dictionary<string, MethodInfo>(); //_l
        private Dictionary<Type, string> _typeEncodedName = new Dictionary<Type, string>(); //_j
    }

    public static class ReflectionHelpers
    {
        public static string GetCustomDescription(object objEnum)
        {
            var fi = objEnum.GetType().GetField(objEnum.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : string.Empty;
        }

        public static string Description(this Enum value)
        {
            return GetCustomDescription(value);
        }
    }
}