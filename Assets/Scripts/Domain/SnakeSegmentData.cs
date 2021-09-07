using System;
using UnityEngine;

namespace Domain
{
    public enum SnakeSegmentType
    {
        Head,
        Body
    }
    
    [Serializable]
    public class SnakeSegmentData
    {
        public Vector2 position;
        public float rotation;
        public SnakeSegmentType type;
        public int ordinalPosition;
    }
}