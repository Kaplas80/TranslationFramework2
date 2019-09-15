using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.EventArgs;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Events;

namespace UnderRailLib.Models
{
    [EncodedTypeName("ED")]
    [Serializable]
    public class EyeDefinition : iNM, iID, iLY, ISerializable
    {
        protected EyeDefinition(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _id = (Guid) info.GetValue("ED:I", typeof(Guid));
                _name = info.GetString("ED:N");
                _layer = info.GetInt32("ED:L");
                SerializationHelper.ReadList("ED:APP", ref _additionalPenetrationPredicates, info);
                SerializationHelper.ReadList("ED:AIP", ref _additionalImpactPredicates, info);
                _customSubscriber = (IEventSubscriber<eaNCEA>) info.GetValue("ED:CS", typeof(IEventSubscriber<eaNCEA>));
                _disableDefaultEventSubscriber = info.GetBoolean("ED:DDES");
                _disableDefaultPenetrationPredicate = info.GetBoolean("ED:DDPP");
                _disableDefaultImpactPredicate = info.GetBoolean("ED:DDIP");
                _range = info.GetSingle("ED:R");
                return;
            }

            if (GetType() == typeof(EyeDefinition))
            {
                _id = (Guid) info.GetValue("_Id", typeof(Guid));
                _name = info.GetString("_Name");
                _layer = info.GetInt32("_Layer");
                _additionalPenetrationPredicates = (List<iPEP>) info.GetValue("_AdditionalPenetrationPredicates", typeof(List<iPEP>));
                _additionalImpactPredicates = (List<iIMP>) info.GetValue("_AdditionalImpactPredicates", typeof(List<iIMP>));
                _customSubscriber = (IEventSubscriber<eaNCEA>) info.GetValue("_CustomSubscriber", typeof(IEventSubscriber<eaNCEA>));
                _disableDefaultEventSubscriber = info.GetBoolean("_DisableDefaultEventSubscriber");
                _disableDefaultPenetrationPredicate = info.GetBoolean("_DisableDefaultPenetrationPredicate");
                _disableDefaultImpactPredicate = info.GetBoolean("_DisableDefaultImpactPredicate");
                _range = info.GetSingle("_Range");
                return;
            }

            _id = (Guid) info.GetValue("EyeDefinition+_Id", typeof(Guid));
            _name = info.GetString("EyeDefinition+_Name");
            _layer = info.GetInt32("EyeDefinition+_Layer");
            _additionalPenetrationPredicates = (List<iPEP>) info.GetValue("EyeDefinition+_AdditionalPenetrationPredicates", typeof(List<iPEP>));
            _additionalImpactPredicates = (List<iIMP>) info.GetValue("EyeDefinition+_AdditionalImpactPredicates", typeof(List<iIMP>));
            _customSubscriber = (IEventSubscriber<eaNCEA>) info.GetValue("EyeDefinition+_CustomSubscriber",
                typeof(IEventSubscriber<eaNCEA>));
            _disableDefaultEventSubscriber = info.GetBoolean("EyeDefinition+_DisableDefaultEventSubscriber");
            _disableDefaultPenetrationPredicate = info.GetBoolean("EyeDefinition+_DisableDefaultPenetrationPredicate");
            _disableDefaultImpactPredicate = info.GetBoolean("EyeDefinition+_DisableDefaultImpactPredicate");
            _range = info.GetSingle("EyeDefinition+_Range");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("ED:I", _id);
            info.AddValue("ED:N", _name);
            info.AddValue("ED:L", _layer);
            SerializationHelper.WriteList("ED:APP", _additionalPenetrationPredicates, info);
            SerializationHelper.WriteList("ED:AIP", _additionalImpactPredicates, info);
            info.AddValue("ED:CS", _customSubscriber);
            info.AddValue("ED:DDES", _disableDefaultEventSubscriber);
            info.AddValue("ED:DDPP", _disableDefaultPenetrationPredicate);
            info.AddValue("ED:DDIP", _disableDefaultImpactPredicate);
            info.AddValue("ED:R", _range);
        }

        private Guid _id;

        private string _name;

        private int _layer;

        private List<iPEP> _additionalPenetrationPredicates = new List<iPEP>();

        private List<iIMP> _additionalImpactPredicates = new List<iIMP>();

        private IEventSubscriber<eaNCEA> _customSubscriber;

        private bool _disableDefaultEventSubscriber;

        private bool _disableDefaultPenetrationPredicate;

        private bool _disableDefaultImpactPredicate;

        private float _range;
    }
}
