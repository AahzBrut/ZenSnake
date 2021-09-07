using Services;
using Settings;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class MouseTargetController : MonoBehaviour
    {
        [SerializeField] private GameSettings gameSettings;
        private GameEventService _gameEventService;
        private Vector2 _targetPosition = Vector2.zero;
        private float _cursorSpeed;

        [Inject]
        public void Construct(
            GameEventService gameEventService
            )
        {
            _gameEventService = gameEventService;
            _gameEventService.MouseMove += OnMouseMove;
        }

        private void OnMouseMove(Vector2 position)
        {
            _targetPosition = position;
        }

        private void Update()
        {
            if (Vector2.Distance(_targetPosition, transform.position) > .001f)
            {
                transform.position = Vector2.Lerp(transform.position, _targetPosition, gameSettings.cursorSpeed * Time.deltaTime);
            }
        }
    }
}