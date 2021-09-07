using UnityEngine;

namespace Controllers
{
    public class GrassController : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        
        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        public void Init(Vector2Int size)
        {
            _renderer.size = size;
        }
    }
}
