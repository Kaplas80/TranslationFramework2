using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnderRailLib.AssemblyResolver;

namespace UnderRailLib.Models
{
    [EncodedTypeName("CA")]
    [Serializable]
    public abstract class CompositeActivity : Activity
    {
        protected CompositeActivity(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                SerializationHelper.ReadList("CA:A", ref _activities, info);
                SerializationHelper.ReadList("CA:CAA", ref _currentAsyncActivities, info);
                SerializationHelper.ReadList("CA:CAA1", ref _completedAsyncActivities, info);
                SerializationHelper.ReadList("CA:FAA", ref _failedAsyncActivities, info);
                SerializationHelper.ReadList("CA:CA", ref _completedActivities, info);
                return;
            }
            if (GetType() == typeof(CompositeActivity))
            {
                _activities = (List<Activity>)info.GetValue("_Activities", typeof(List<Activity>));
                _currentAsyncActivities = (List<Activity>)info.GetValue("_CurrentAsyncActivities", typeof(List<Activity>));
                _completedAsyncActivities = (List<Activity>)info.GetValue("_CompletedAsyncActivities", typeof(List<Activity>));
                _failedAsyncActivities = (List<Activity>)info.GetValue("_FailedAsyncActivities", typeof(List<Activity>));
                _completedActivities = (List<Activity>)info.GetValue("_CompletedActivities", typeof(List<Activity>));
                return;
            }
            _activities = (List<Activity>)info.GetValue("CompositeActivity+_Activities", typeof(List<Activity>));
            _currentAsyncActivities = (List<Activity>)info.GetValue("CompositeActivity+_CurrentAsyncActivities", typeof(List<Activity>));
            _completedAsyncActivities = (List<Activity>)info.GetValue("CompositeActivity+_CompletedAsyncActivities", typeof(List<Activity>));
            _failedAsyncActivities = (List<Activity>)info.GetValue("CompositeActivity+_FailedAsyncActivities", typeof(List<Activity>));
            _completedActivities = (List<Activity>)info.GetValue("CompositeActivity+_CompletedActivities", typeof(List<Activity>));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            SerializationHelper.WriteList("CA:A", _activities, info);
            SerializationHelper.WriteList("CA:CAA", _currentAsyncActivities, info);
            SerializationHelper.WriteList("CA:CAA1", _completedAsyncActivities, info);
            SerializationHelper.WriteList("CA:FAA", _failedAsyncActivities, info);
            SerializationHelper.WriteList("CA:CA", _completedActivities, info);
        }

        private List<Activity> _activities = new List<Activity>();

        private List<Activity> _currentAsyncActivities = new List<Activity>();

        private List<Activity> _completedAsyncActivities = new List<Activity>();

        private List<Activity> _failedAsyncActivities = new List<Activity>();

        private List<Activity> _completedActivities = new List<Activity>();
    }
}
