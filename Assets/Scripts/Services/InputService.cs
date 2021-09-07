using System;
using UnityEngine;
using Zenject;

namespace Services
{
    public class InputService : MonoBehaviour
    {
        private GameEventService _gameEventService;
        private Vector3 _prevMousePosition = Vector3.positiveInfinity;
        private Camera _camera;

        [Inject]
        public void Construct(GameEventService gameEventService)
        {
            _gameEventService = gameEventService;
        }

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void LateUpdate()
        {
            var mousePosition = Input.mousePosition;
            if (Vector2.Distance(mousePosition, _prevMousePosition) > .05f)
            {
                _gameEventService.MouseMove.Invoke(_camera.ScreenToWorldPoint(mousePosition));
                _prevMousePosition = mousePosition;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _gameEventService.SpaceBarPressed.Invoke();
            }
        }
    }
}