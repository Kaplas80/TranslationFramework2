using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("KI")]
    [Serializable]
    public sealed class KnowledgeItem : ISerializable
    {
        private KnowledgeItem(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                SerializationHelper.ReadDictionary("KI:S", ref Sections, info);
                return;
            }
            if (GetType() == typeof(KnowledgeItem))
            {
                Sections = (Dictionary<string, string>)info.GetValue("Sections", typeof(Dictionary<string, string>));
                return;
            }
            Sections = (Dictionary<string, string>)info.GetValue("KnowledgeItem+Sections", typeof(Dictionary<string, string>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            SerializationHelper.WriteDictionary("KI:S", Sections, info);
        }

        public Dictionary<string, string> Sections = new Dictionary<string, string>();
    }
}
