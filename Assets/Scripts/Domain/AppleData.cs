using System;
using UnityEngine;

namespace Domain
{
    [Serializable]
    public class AppleData
    {
        public Vector2 position;
        public AppleState state;
        
        public enum AppleState
        {
            Whole,
            Eaten
        }
    }
}