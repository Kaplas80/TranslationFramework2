using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models.Interfaces;
using UnderRailLib.Ouroboros.Common.Data;

namespace UnderRailLib.Models
{
    [EncodedTypeName("A6")]
    [Serializable]
    public class Area : E2, iIC
    {
        protected Area(SerializationInfo info, StreamingContext ctx) : base(info, ctx)
        {
            if (DataModelVersion.MajorVersion != 0)
            {
                _tiles = (T1[,]) info.GetValue("A6:T", typeof(T1[,]));
                _entities = (List<E4>) info.GetValue("A6:E", typeof(List<E4>));
                SerializationHelper.ReadList("A6:T1", ref _triggers, info);
                _zone = (Zone) info.GetValue("A6:Z", typeof(Zone));
                _size = (Size) info.GetValue("A6:S", typeof(Size));
                _markedForUndeployment = (Queue<E4>) info.GetValue("A6:MFU", typeof(Queue<E4>));
                _minimalLightning = (Color) info.GetValue("A6:ML", typeof(Color));
                _maximalLightning = (Color) info.GetValue("A6:ML1", typeof(Color));
                _startingLightning = (Color) info.GetValue("A6:SL", typeof(Color));
                _softLightAreas = (SoftLightAreaContainer) info.GetValue("A6:SLA", typeof(SoftLightAreaContainer));
                _divinations = (DivinationContainer) info.GetValue("A6:D", typeof(DivinationContainer));
                if (DataModelVersion.MajorVersion >= 28)
                {
                    _cp = (PropertyCollection) info.GetValue("A6:CP", typeof(PropertyCollection));
                    return;
                }

                _cp = new PropertyCollection();
            }
            else
            {
                if (GetType() == typeof(Area))
                {
                    _tiles = (T1[,]) info.GetValue("_Tiles", typeof(T1[,]));
                    _entities = (List<E4>) info.GetValue("_Entities", typeof(List<E4>));
                    _triggers = (List<Trigger>) info.GetValue("_Triggers", typeof(List<Trigger>));
                    _zone = (Zone) info.GetValue("_Zone", typeof(Zone));
                    _size = (Size) info.GetValue("_Size", typeof(Size));
                    _markedForUndeployment = (Queue<E4>) info.GetValue("_MarkedForUndeployment", typeof(Queue<E4>));
                    _minimalLightning = (Color) info.GetValue("_MinimalLightning", typeof(Color));
                    _maximalLightning = (Color) info.GetValue("_MaximalLightning", typeof(Color));
                    _startingLightning = (Color) info.GetValue("_StartingLightning", typeof(Color));
                    _softLightAreas = (SoftLightAreaContainer) info.GetValue("_SoftLightAreas", typeof(SoftLightAreaContainer));
                    _divinations = (DivinationContainer) info.GetValue("_Divinations", typeof(DivinationContainer));
                    return;
                }

                _tiles = (T1[,]) info.GetValue("Area+_Tiles", typeof(T1[,]));
                _entities = (List<E4>) info.GetValue("Area+_Entities", typeof(List<E4>));
                _triggers = (List<Trigger>) info.GetValue("Area+_Triggers", typeof(List<Trigger>));
                _zone = (Zone) info.GetValue("Area+_Zone", typeof(Zone));
                _size = (Size) info.GetValue("Area+_Size", typeof(Size));
                _markedForUndeployment = (Queue<E4>) info.GetValue("Area+_MarkedForUndeployment", typeof(Queue<E4>));
                _minimalLightning = (Color) info.GetValue("Area+_MinimalLightning", typeof(Color));
                _maximalLightning = (Color) info.GetValue("Area+_MaximalLightning", typeof(Color));
                _startingLightning = (Color) info.GetValue("Area+_StartingLightning", typeof(Color));
                _softLightAreas = (SoftLightAreaContainer) info.GetValue("Area+_SoftLightAreas", typeof(SoftLightAreaContainer));
                _divinations = (DivinationContainer) info.GetValue("Area+_Divinations", typeof(DivinationContainer));
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
        {
            base.GetObjectData(info, ctx);
            info.AddValue("A6:T", _tiles);
            info.AddValue("A6:E", _entities);
            SerializationHelper.WriteList("A6:T1", _triggers, info);
            info.AddValue("A6:Z", _zone);
            info.AddValue("A6:S", _size);
            info.AddValue("A6:MFU", _markedForUndeployment);
            info.AddValue("A6:ML", _minimalLightning);
            info.AddValue("A6:ML1", _maximalLightning);
            info.AddValue("A6:SL", _startingLightning);
            info.AddValue("A6:SLA", _softLightAreas);
            info.AddValue("A6:D", _divinations);
            info.AddValue("A6:CP", _cp);
        }

        private T1[,] _tiles;

        private List<E4> _entities;

        private List<Trigger> _triggers;

        private Zone _zone;

        private Size _size;

        private Queue<E4> _markedForUndeployment;

        private Color _minimalLightning;

        private Color _maximalLightning;

        private Color _startingLightning;

        private SoftLightAreaContainer _softLightAreas;

        private DivinationContainer _divinations;

        private PropertyCollection _cp;
    }
}
