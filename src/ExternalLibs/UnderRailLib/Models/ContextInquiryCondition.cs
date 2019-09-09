using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CIC")]
    [Serializable]
    public abstract class ContextInquiryCondition : Condition
    {
        protected ContextInquiryCondition(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _path = info.GetString("CIC:P");
                _allowDefaulting = info.GetBoolean("CIC:AD");
                return;
            }
            if (GetType() == typeof(ContextInquiryCondition))
            {
                _path = info.GetString("_Path");
                _allowDefaulting= info.GetBoolean("_AllowDefaulting");
                return;
            }
            _path = info.GetString("ContextInquiryCondition+_Path");
            _allowDefaulting = info.GetBoolean("ContextInquiryCondition+_AllowDefaulting");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("CIC:P", _path);
            info.AddValue("CIC:AD", _allowDefaulting);
        }

        private string _path;
        private bool _allowDefaulting;
    }
}
