using Domain;
using UnityEngine;

namespace Controllers
{
    public class ObstacleController : MonoBehaviour
    {
        public void Init(ObstacleData obstacleData)
        {
            transform.position = obstacleData.position;
            transform.localScale = obstacleData.size;
        }
    }
}