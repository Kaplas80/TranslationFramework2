using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Aspects;

namespace UnderRailLib.Models
{
    [EncodedTypeName("GICEA")]
    [Serializable]
    public sealed class GICEA : Aspect<LE2>, iINA, iCHA
    {
        private GICEA(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _opprtunitySet = (ItemOpportunitySet)info.GetValue("GICEA:OS", typeof(ItemOpportunitySet));
                _generated = info.GetBoolean("GICEA:G");
            }
            else
            {
                _opprtunitySet = (info.GetValue("_OpportunitySet", typeof(ItemOpportunitySet)) as ItemOpportunitySet);
                _generated = info.GetBoolean("_Generated");
            }
            if (DataModelVersion.MinorVersion >= 146)
            {
                _d = (info.GetValue("GICEA:D", typeof(double?)) as double?);
            }
            if (DataModelVersion.MinorVersion >= 507)
            {
                _s = info.GetInt32("GICEA:S");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("GICEA:OS", _opprtunitySet);
            info.AddValue("GICEA:G", _generated);
            info.AddValue("GICEA:D", _d);
            info.AddValue("GICEA:S", _s);
        }

        private ItemOpportunitySet _opprtunitySet = new ItemOpportunitySet();

        private bool _generated;

        private double? _d;

        private int _s;
    }
}
