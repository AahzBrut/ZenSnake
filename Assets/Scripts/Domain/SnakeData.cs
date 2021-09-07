using System;

namespace Domain
{
    [Serializable]
    public class SnakeData
    {
        public HashMap<int,SnakeSegmentData> snakeSegments = new HashMap<int, SnakeSegmentData>();
    }
}