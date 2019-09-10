using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Data;

namespace UnderRailLib.TimelapseVertigo.Rules.Items
{
    [EncodedTypeName("LIDP")]
    [Serializable]
    public sealed class LibraryItemDefinitionProvider<T> : TypedObjectReference<T>, ItemDefinitionProvider, iITLDFP
        where T : Item
    {
        private LibraryItemDefinitionProvider(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _path = info.GetString("LIDP:P");
                return;
            }

            if (GetType() == typeof(LibraryItemDefinitionProvider<T>))
            {
                _path = info.GetString("_Path");
                return;
            }

            _path = info.GetString("LibraryItemDefinitionProvider`1+_Path");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("LIDP:P", _path);
        }

        private string _path;
    }
}
