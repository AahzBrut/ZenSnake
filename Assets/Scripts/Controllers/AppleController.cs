using Domain;
using Settings;
using UnityEditorInternal;
using UnityEngine;

namespace Controllers
{
    public class AppleController : MonoBehaviour
    {
        [SerializeField] private Sprite wholeSprite;
        [SerializeField] private Sprite eatenSprite;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private GameSettings gameSettings;
        
        private AppleData _data;
        private float _timeout;
        public bool IsEaten => _data.state == AppleData.AppleState.Eaten;

        public void Init(AppleData apple)
        {
            _data = apple;
            spriteRenderer.sprite = _data.state == AppleData.AppleState.Whole ? wholeSprite : eatenSprite;
        }

        private void Update()
        {
            if (_data.state == AppleData.AppleState.Eaten)
            {
                _timeout -= Time.deltaTime;
                if (_timeout <= 0f)
                {
                    _data.state = AppleData.AppleState.Whole;
                    spriteRenderer.sprite = wholeSprite;
                }
            }
        }

        public void Eat()
        {
            _data.state = AppleData.AppleState.Eaten;
            spriteRenderer.sprite = eatenSprite;
            _timeout = gameSettings.appleEatenTimeout;
        }
    }
}