using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Enums;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CEA3")]
    [Serializable]
    public class CEA3 : OCUEA, iCA, iITEXT, iMAIA
    {
        protected CEA3(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _itemContainer = (IItemContainer)info.GetValue("CEA3:IC", typeof(IItemContainer));
                _cpm = (eCONPM)info.GetValue("CEA3:CPM", typeof(eCONPM));
                if (DataModelVersion.MinorVersion >= 241)
                {
                    _i = info.GetBoolean("CEA3:I");
                }
            }
            else
            {
                _itemContainer = (info.GetValue("_ItemContainer", typeof(IItemContainer)) as IItemContainer);
                try
                {
                    _cpm = (eCONPM)info.GetValue("CEA_CPM", typeof(eCONPM));
                }
                catch
                {
                }
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CEA3:IC", _itemContainer);
            info.AddValue("CEA3:CPM", _cpm);
            info.AddValue("CEA3:I", _i);
        }

        private IItemContainer _itemContainer;

        private eCONPM _cpm;

        internal bool _i;
    }
}
