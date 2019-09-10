using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;

namespace UnderRailLib.Models
{
    [EncodedTypeName("GIJ")]
    [Serializable]
    public sealed class GiveItemsJob : Job
    {
        private GiveItemsJob(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _staticItems = (ItemContainer) info.GetValue("GIJ:SI", typeof(ItemContainer));
                _dynamicItems = (ItemOpportunitySet) info.GetValue("GIJ:DI", typeof(ItemOpportunitySet));
                _giveTo = (eSPCJBT) info.GetValue("GIJ:GT", typeof(eSPCJBT));
                _publishEventsForPlayer = info.GetBoolean("GIJ:PEFP");
                if (DataModelVersion.MinorVersion >= 519)
                {
                    _autoEquip = (info.GetValue("GIJ:AE", typeof(AutoEquip?)) as AutoEquip?);
                }
            }
            else
            {
                if (GetType() == typeof(GiveItemsJob))
                {
                    _staticItems = (ItemContainer) info.GetValue("_StaticItems", typeof(ItemContainer));
                    _dynamicItems = (ItemOpportunitySet) info.GetValue("_DynamicItems", typeof(ItemOpportunitySet));
                    _giveTo = (eSPCJBT) info.GetValue("_GiveTo", typeof(eSPCJBT));
                    _publishEventsForPlayer = info.GetBoolean("_PublishEventsForPlayer");
                    return;
                }

                _staticItems = (ItemContainer) info.GetValue("GiveItemsJob+_StaticItems", typeof(ItemContainer));
                _dynamicItems = (ItemOpportunitySet) info.GetValue("GiveItemsJob+_DynamicItems", typeof(ItemOpportunitySet));
                _giveTo = (eSPCJBT) info.GetValue("GiveItemsJob+_GiveTo", typeof(eSPCJBT));
                _publishEventsForPlayer = info.GetBoolean("GiveItemsJob+_PublishEventsForPlayer");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("GIJ:SI", _staticItems);
            info.AddValue("GIJ:DI", _dynamicItems);
            info.AddValue("GIJ:GT", _giveTo);
            info.AddValue("GIJ:PEFP", _publishEventsForPlayer);
            if (DataModelVersion.MinorVersion >= 519)
            {
                info.AddValue("GIJ:AE", _autoEquip);
            }
        }

        private ItemContainer _staticItems = new ItemContainer();

        private ItemOpportunitySet _dynamicItems = new ItemOpportunitySet();

        private eSPCJBT _giveTo = eSPCJBT.c;

        private bool _publishEventsForPlayer = true;

        private AutoEquip? _autoEquip;
    }
}
