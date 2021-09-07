using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        public Vector2Int mazeSize;
        public float snakeSpeed;
        public float snakeAngularSpeed;
        public int numberOfObstacles;
        public int numberOfApples;
        public float cursorSpeed;
        public float snakeSegmentSize;
        public float appleEatenTimeout;
        public float cameraSpeed;
    }
}