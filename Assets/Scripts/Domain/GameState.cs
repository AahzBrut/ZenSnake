using System;
using System.Collections.Generic;
using UnityEngine;

namespace Domain
{
    [Serializable]
    public class GameState
    {
        public Vector2Int mazeSize;
        public float snakeSpeed;
        public float snakeAngularSpeed;
        public int numberOfObstacles;
        public int numberOfApples;
        public List<AppleData> apples;
        public List<ObstacleData> obstacles;
        public SnakeData snake;
        public int score;
    }
}