using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models;

namespace UnderRailLib
{
    public static class SerializationHelper
    {
        public static void WriteDictionary<TKey, TValue>(string dictionaryName, Dictionary<TKey, TValue> dictionary, SerializationInfo info)
        {
            if (dictionary != null)
            {
                info.AddValue(dictionaryName + ":Count", dictionary.Count);
                var num = 0;
                using (var enumerator = dictionary.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        var keyValuePair = enumerator.Current;
                        info.AddValue(string.Concat(dictionaryName, ":", num, ":Key"), keyValuePair.Key);
                        info.AddValue(string.Concat(dictionaryName, ":", num, ":Value"), keyValuePair.Value);
                        num++;
                    }

                    return;
                }
            }

            info.AddValue(dictionaryName + ":Count", -1);
        }

        public static void WriteDataModelTypeKeyedDictionary<TValue>(string dictionaryName, Dictionary<Type, TValue> dictionary, SerializationInfo info)
        {
            if (dictionary != null)
            {
                info.AddValue(dictionaryName + ":C", dictionary.Count);
                var num = 0;
                using (var enumerator = dictionary.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        var keyValuePair = enumerator.Current;
                        a(string.Concat(dictionaryName, ":", num, ":K"), keyValuePair.Key, info);
                        info.AddValue(string.Concat(dictionaryName, ":", num, ":V"), keyValuePair.Value);
                        num++;
                    }

                    return;
                }
            }

            info.AddValue(dictionaryName + ":C", -1);
        }

        public static void WriteDataModelTypeKeyedPairList<TValue>(string pairListName, List<KeyValuePair<Type, TValue>> pairList, SerializationInfo info)
        {
            if (pairList != null)
            {
                info.AddValue(pairListName + ":C", pairList.Count);
                var num = 0;
                using (var enumerator = pairList.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        var keyValuePair = enumerator.Current;
                        a(string.Concat(pairListName, ":", num, ":K"), keyValuePair.Key, info);
                        info.AddValue(string.Concat(pairListName, ":", num, ":V"), keyValuePair.Value);
                        num++;
                    }

                    return;
                }
            }

            info.AddValue(pairListName + ":C", -1);
        }

        public static void WriteList<T>(string listName, List<T> list, SerializationInfo info)
        {
            if (list != null)
            {
                info.AddValue(listName + ":Count", list.Count);
                for (var i = 0; i < list.Count; i++)
                {
                    info.AddValue(listName + ":" + i, list[i]);
                }

                return;
            }

            info.AddValue(listName + ":Count", -1);
        }

        public static void WriteCollection<T>(string listName, Collection<T> col, SerializationInfo info)
        {
            if (col != null)
            {
                info.AddValue(listName + ":Count", col.Count);
                for (var i = 0; i < col.Count; i++)
                {
                    info.AddValue(listName + ":" + i, col[i]);
                }

                return;
            }

            info.AddValue(listName + ":Count", -1);
        }

        public static void WriteArray<T>(string arrayName, T[] array, SerializationInfo info)
        {
            if (array != null)
            {
                info.AddValue(arrayName + ":Length", array.Length);
                for (var i = 0; i < array.Length; i++)
                {
                    info.AddValue(arrayName + ":" + i, array[i]);
                }

                return;
            }

            info.AddValue(arrayName + ":Length", -1);
        }

        public static void WriteArray<T>(string arrayName, T[,] array, SerializationInfo info)
        {
            if (array != null)
            {
                var lengthX = array.GetLength(0);
                var lengthY = array.GetLength(1);
                info.AddValue(arrayName + ":LengthX", lengthX);
                info.AddValue(arrayName + ":LengthY", lengthY);
                for (var i = 0; i < lengthX; i++)
                {
                    for (var j = 0; j < lengthY; j++)
                    {
                        info.AddValue(string.Concat(arrayName, ":", i, ",", j), array[i, j]);
                    }
                }

                return;
            }

            info.AddValue(arrayName + ":LengthX", -1);
        }

        public static void a(string tag, Type type, SerializationInfo info)
        {
            Binder.Resolver.GetName(type, out var value, out var value2);
            info.AddValue(tag, value);
            info.AddValue(tag + ":A", value2);
        }

        public static void WriteEvent<T>(string eventName, EventHandler<T> eventHandler, SerializationInfo info)
            where T : EventArgs
        {
            EventSerializationInfo byv = null;
            if (eventHandler != null)
            {
                byv = new EventSerializationInfo();
                foreach (var deleg in eventHandler.GetInvocationList())
                {
                    var bqk = GetDelegateInfo(deleg);
                    if (bqk != null)
                    {
                        byv.DelegateInfoList.Add(bqk);
                    }
                }
            }

            info.AddValue(eventName, byv, typeof(EventSerializationInfo));
        }

        public static void ReadDictionary<TKey, TValue>(string dictionaryName, ref Dictionary<TKey, TValue> dictionary, SerializationInfo info)
        {
            var count = info.GetInt32(dictionaryName + ":Count");
            if (count != -1)
            {
                if (dictionary == null)
                {
                    dictionary = new Dictionary<TKey, TValue>();
                }

                for (var i = 0; i < count; i++)
                {
                    var key = (TKey) info.GetValue(string.Concat(dictionaryName, ":", i, ":Key"), typeof(TKey));
                    var value = info.GetValue(string.Concat(dictionaryName, ":", i, ":Value"), typeof(TValue));
                    TValue value2;
                    if (value is TValue)
                    {
                        value2 = (TValue) value;
                    }
                    else
                    {
                        value2 = default;
                    }

                    dictionary.Add(key, value2);
                }
            }
        }

        public static void ReadDataModelTypeKeyedDictionary<TValue>(string dictionaryName, ref Dictionary<Type, TValue> dictionary, SerializationInfo info)
        {
            var count = info.GetInt32(dictionaryName + ":C");
            if (count != -1)
            {
                if (dictionary == null)
                {
                    dictionary = new Dictionary<Type, TValue>();
                }

                for (var i = 0; i < count; i++)
                {
                    var key = a(string.Concat(dictionaryName, ":", i, ":K"), info);
                    var value = (TValue) info.GetValue(string.Concat(dictionaryName, ":", i, ":V"), typeof(TValue));
                    dictionary.Add(key, value);
                }
            }
        }

        public static void ReadDataModelTypeKeyedPairList<TValue>(string pairListName, ref List<KeyValuePair<Type, TValue>> pairList, SerializationInfo info)
        {
            var count = info.GetInt32(pairListName + ":C");
            if (count != -1)
            {
                if (pairList == null)
                {
                    pairList = new List<KeyValuePair<Type, TValue>>();
                }

                for (var i = 0; i < count; i++)
                {
                    var key = a(string.Concat(pairListName, ":", i, ":K"), info);
                    var value = (TValue) info.GetValue(string.Concat(pairListName, ":", i, ":V"), typeof(TValue));
                    pairList.Add(new KeyValuePair<Type, TValue>(key, value));
                }
            }
        }

        public static void ReadList<T>(string listName, ref List<T> list, SerializationInfo info)
        {
            var count = info.GetInt32(listName + ":Count");
            if (count != -1)
            {
                if (list == null)
                {
                    list = new List<T>();
                }

                for (var i = 0; i < count; i++)
                {
                    var value = info.GetValue(listName + ":" + i, typeof(T));
                    T item;
                    if (value is T)
                    {
                        item = (T) value;
                    }
                    else
                    {
                        item = default;
                    }

                    list.Add(item);
                }
            }
        }

        public static void ReadCollection<T>(string listName, ref Collection<T> col, SerializationInfo info)
        {
            var count = info.GetInt32(listName + ":Count");
            if (count != -1)
            {
                if (col == null)
                {
                    col = new Collection<T>();
                }

                for (var i = 0; i < count; i++)
                {
                    var value = info.GetValue(listName + ":" + i, typeof(T));
                    T item;
                    if (value is T)
                    {
                        item = (T) value;
                    }
                    else
                    {
                        item = default;
                    }

                    col.Add(item);
                }
            }
        }

        public static void ReadArray<T>(string arrayName, ref T[] array, SerializationInfo info)
        {
            var length = info.GetInt32(arrayName + ":Length");
            if (length != -1)
            {
                if (array == null || array.Length != length)
                {
                    array = new T[length];
                }

                for (var i = 0; i < length; i++)
                {
                    var value = info.GetValue(arrayName + ":" + i, typeof(T));
                    T t;
                    if (value is T)
                    {
                        t = (T) value;
                    }
                    else
                    {
                        t = default;
                    }

                    array[i] = t;
                }
            }
        }

        public static void ReadArray<T>(string arrayName, ref T[,] array, SerializationInfo info)
        {
            var lengthX = info.GetInt32(arrayName + ":LengthX");
            if (lengthX != -1)
            {
                var lengthY = info.GetInt32(arrayName + ":LengthY");
                if (array == null || array.GetLength(0) != lengthX || array.GetLength(1) != lengthY)
                {
                    array = new T[lengthX, lengthY];
                }

                for (var i = 0; i < lengthX; i++)
                {
                    for (var j = 0; j < lengthY; j++)
                    {
                        var value = info.GetValue(string.Concat(arrayName, ":", i, ",", j), typeof(T));
                        T t;
                        if (value is T)
                        {
                            t = (T) value;
                        }
                        else
                        {
                            t = default;
                        }

                        array[i, j] = t;
                    }
                }
            }
        }

        public static Type a(string tag, SerializationInfo info)
        {
            if (DataModelVersion.MajorVersion >= 4)
            {
                return Binder.Resolver.GetType(info.GetString(tag), info.GetString(tag + ":A"));
            }

            return Binder.Resolver.GetType(info.GetString(tag), "X");
        }

        public static void ReadEvent<T>(string eventName, ref EventHandler<T> eventHandler, SerializationInfo info)
            where T : EventArgs
        {
            if (info.GetValue(eventName, typeof(EventSerializationInfo)) is EventSerializationInfo esi && esi.DelegateInfoList != null)
            {
                foreach (var di in esi.DelegateInfoList)
                {
                    var deleg = GetDelegate(di);
                    if (deleg != null)
                    {
                        eventHandler = (Delegate.Combine(eventHandler, deleg) as EventHandler<T>);
                    }
                }
            }
        }

        private static DelegateInfo GetDelegateInfo(Delegate deleg)
        {
            var text = Binder.Resolver.GetMethodName(deleg.Method);
            if (text != null)
            {
                return new DelegateInfo
                {
                    DelegateType = deleg.GetType(),
                    Target = deleg.Target,
                    MethodName = text
                };
            }

            throw new SerializationException(
                $"Failed to retrieve method element name for method '{deleg.Method}' declared in type '{deleg.Method.DeclaringType}'.");
        }

        private static Delegate GetDelegate(DelegateInfo info)
        {
            if (info.DelegateType == null)
            {
                throw new SerializationException("Delegate type not found.");
            }

            var methodInfo = Binder.Resolver.GetMethodInfo(info.MethodName);
            if (methodInfo == null)
            {
                throw new SerializationException("Data model method not found: " + info.MethodName);
            }

            Delegate result;
            if (methodInfo.IsStatic)
            {
                result = Delegate.CreateDelegate(info.DelegateType, methodInfo);
            }
            else
            {
                if (info.Target == null)
                {
                    throw new SerializationException("Target not found for delegate. Method: " + info.MethodName);
                }

                result = Delegate.CreateDelegate(info.DelegateType, info.Target, methodInfo);
            }

            return result;
        }
    }
}