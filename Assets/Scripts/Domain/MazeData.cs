using System;
using System.Collections.Generic;

namespace Domain
{
    [Serializable]
    public class MazeData
    {
        public List<ObstacleData> obstacles = new List<ObstacleData>();
        public List<AppleData> apples = new List<AppleData>();
        public SnakeData snakeData;
    }
}