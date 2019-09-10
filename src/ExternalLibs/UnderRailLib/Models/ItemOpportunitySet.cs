using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("IOS")]
    [Serializable]
    public sealed class ItemOpportunitySet : ISerializable
    {
        public ItemOpportunitySet()
        {

        }
        private ItemOpportunitySet(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                SerializationHelper.ReadList("IOS:O", ref _opportunities, info);
                if (DataModelVersion.MinorVersion >= 406)
                {
                    _bl = (info.GetValue("IOS:BL", typeof(int?)) as int?);
                    _md = (info.GetValue("IOS:MD", typeof(int?)) as int?);
                    _mo = (info.GetValue("IOS:MO", typeof(bool?)) as bool?);
                }

                if (DataModelVersion.MinorVersion >= 427)
                {
                    _i = (info.GetValue("IOS:I", typeof(bool?)) as bool?);
                }
            }
            else
            {
                if (GetType() == typeof(ItemOpportunitySet))
                {
                    _opportunities = (List<ItemOpportunity>) info.GetValue("_Opportunities", typeof(List<ItemOpportunity>));
                    return;
                }

                _opportunities = (List<ItemOpportunity>) info.GetValue("ItemOpportunitySet+_Opportunities", typeof(List<ItemOpportunity>));
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            SerializationHelper.WriteList("IOS:O", _opportunities, info);
            if (DataModelVersion.MinorVersion >= 406)
            {
                info.AddValue("IOS:BL", _bl);
                info.AddValue("IOS:MD", _md);
                info.AddValue("IOS:MO", _mo);
            }

            if (DataModelVersion.MinorVersion >= 427)
            {
                info.AddValue("IOS:I", _i);
            }
        }

        private List<ItemOpportunity> _opportunities = new List<ItemOpportunity>();

        private int? _bl;

        private int? _md;

        private bool? _mo;

        private bool? _i;
    }
}
