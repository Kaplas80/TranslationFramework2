using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SCA")]
    [Serializable]
    public class SequentialCompositeActivity : CompositeActivity
    {
        protected SequentialCompositeActivity(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _current = info.GetInt32("SCA:C");
                return;
            }
            if (GetType() == typeof(SequentialCompositeActivity))
            {
                _current = info.GetInt32("_Current");
                return;
            }
            _current = info.GetInt32("SequentialCompositeActivity+_Current");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SCA:C", _current);
        }

        private int _current;
    }
}
