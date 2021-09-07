using System;
using System.Collections.Generic;
using Domain;
using UnityEngine;

namespace Services
{
    public class GameEventService : MonoBehaviour
    {
        public Action<Vector2> MouseMove;
        public Action SpaceBarPressed;
        public Action<GameState> MazeInitComplete;
        public Action<List<ObstacleData>> ObstaclesInitComplete;
        public Action<SnakeData> SnakeInitComplete;
        public Action<List<AppleData>> ApplesInitComplete;
        public Action CollideWithApple;
        public Action CollideWithObstacle;
        public Action<SnakeSegmentData> AddSnakeSegment;
        public Action<int> ScoreChanged;
        public Action<Transform> TargetAcquired;
        public Action Pause;
        public Action UnPause;
        public Action GameStart;
        public Action GameOver;
        public Action GameInitCompleted;
    }
}