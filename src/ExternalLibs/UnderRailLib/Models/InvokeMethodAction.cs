using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("IMA")]
    [Serializable]
    public sealed class InvokeMethodAction : BaseAction
    {
        private InvokeMethodAction(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _targetObjectPath = info.GetString("IMA:TOP");
                _methodName = info.GetString("IMA:MN");
                return;
            }

            if (GetType() == typeof(InvokeMethodAction))
            {
                _targetObjectPath = info.GetString("_TargetObjectPath");
                _methodName = info.GetString("_MethodName");
                return;
            }

            _targetObjectPath = info.GetString("InvokeMethodAction+_TargetObjectPath");
            _methodName = info.GetString("InvokeMethodAction+_MethodName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("IMA:TOP", _targetObjectPath);
            info.AddValue("IMA:MN", _methodName);
        }

        private string _targetObjectPath;

        private string _methodName;
    }
}
