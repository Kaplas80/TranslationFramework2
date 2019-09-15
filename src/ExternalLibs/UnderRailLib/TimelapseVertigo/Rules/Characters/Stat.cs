using System;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.EventArgs;
using UnderRailLib.Models.Interfaces;

namespace UnderRailLib.TimelapseVertigo.Rules.Characters
{
    [EncodedTypeName("S4")]
    [Serializable]
    public abstract class Stat<T> : ISerializable where T : struct
    {
        protected Stat(SerializationInfo info, StreamingContext ctx)
        {
            if (DataModelVersion.MinorVersion != 0)
            {
                _container = (iCHP)info.GetValue("S4:C", typeof(iCHP));
                _value = (T)info.GetValue("S4:V", typeof(T));
                _modifiedValue = (T)info.GetValue("S4:MV", typeof(T));
                if (DataModelVersion.MajorVersion >= 5)
                {
                    SerializationHelper.ReadEvent("S4:VC", ref _valueChanged, info);
                    SerializationHelper.ReadEvent("S4:MVC", ref _modifiedValueChanged, info);
                    return;
                }
                _valueChanged = (EventHandler<eaPCEA>)info.GetValue("S4:VC", typeof(EventHandler<eaPCEA>));
                _modifiedValueChanged = (EventHandler<eaPCEA>)info.GetValue("S4:MVC", typeof(EventHandler<eaPCEA>));
            }
            else
            {
                if (GetType() == typeof(Stat<T>))
                {
                    _container = (iCHP)info.GetValue("_Container", typeof(iCHP));
                    _value = (T)info.GetValue("_Value", typeof(T));
                    _modifiedValue = (T)info.GetValue("_ModifiedValue", typeof(T));
                    _valueChanged = (EventHandler<eaPCEA>)info.GetValue("ValueChanged", typeof(EventHandler<eaPCEA>));
                    _modifiedValueChanged = (EventHandler<eaPCEA>)info.GetValue("ModifiedValueChanged", typeof(EventHandler<eaPCEA>));
                    return;
                }
                _container = (iCHP)info.GetValue("Stat`1+_Container", typeof(iCHP));
                _value = (T)info.GetValue("Stat`1+_Value", typeof(T));
                _modifiedValue = (T)info.GetValue("Stat`1+_ModifiedValue", typeof(T));
                _valueChanged = (EventHandler<eaPCEA>)info.GetValue("Stat`1+ValueChanged", typeof(EventHandler<eaPCEA>));
                _modifiedValueChanged = (EventHandler<eaPCEA>)info.GetValue("Stat`1+ModifiedValueChanged", typeof(EventHandler<eaPCEA>));
            }
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            info.AddValue("S4:C", _container);
            info.AddValue("S4:V", _value);
            info.AddValue("S4:MV", _modifiedValue);
            SerializationHelper.WriteEvent("S4:VC", _valueChanged, info);
            SerializationHelper.WriteEvent("S4:MVC", _modifiedValueChanged, info);
        }

        private iCHP _container;

        private T _value;

        private T _modifiedValue;

        private EventHandler<eaPCEA> _valueChanged;

        private EventHandler<eaPCEA> _modifiedValueChanged;
    }
}
