using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.TimelapseVertigo.Rules.Items
{
    [EncodedTypeName("IIDP")]
    [Serializable]
    public sealed class IndependentItemDefinitionProvider<T> : iITLDFP, ItemDefinitionProvider, ISerializable where T : Item
    {
        private IndependentItemDefinitionProvider(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _d = (Item) info.GetValue("IIDP:D", typeof(Item));
                if (DataModelVersion.MinorVersion >= 350)
                {
                    _version = (Version) info.GetValue("IIDP:V", typeof(Version));
                    return;
                }

                _version = new Version(0, 0, 0, 0);
            }
            else
            {
                if (GetType() == typeof(IndependentItemDefinitionProvider<T>))
                {
                    _d = (Item) info.GetValue("_Definition", typeof(Item));
                    return;
                }

                _d = (Item) info.GetValue("IndependentItemDefinitionProvider`1+_Definition", typeof(Item));
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("IIDP:D", _d);
            if (DataModelVersion.MinorVersion >= 350)
            {
                info.AddValue("IIDP:V", _version);
            }
        }

        private Item _d;

        private Version _version = new Version(1, 1, 0, 12);
    }
}
