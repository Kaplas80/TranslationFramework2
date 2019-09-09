using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("SFVA")]
    [Serializable]
    public abstract class SetFieldValueAction : BaseAction, ISerializable
    {
        protected SetFieldValueAction(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _path = info.GetString("SFVA:P");
                return;
            }
            if (GetType() == typeof(SetFieldValueAction))
            {
                _path = info.GetString("_Path");
                return;
            }
            _path = info.GetString("SetFieldValueAction+_Path");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("SFVA:P", _path);
        }

        private string _path;
    }
}
