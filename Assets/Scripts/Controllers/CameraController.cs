using Services;
using Settings;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private GameSettings gameSettings;
        private Transform _target;
        private GameEventService _gameEventService;
        
        [Inject]
        public void Construct(GameEventService gameEventService)
        {
            _gameEventService = gameEventService;
            _gameEventService.TargetAcquired += OnTargetAcquired;
        }

        private void OnTargetAcquired(Transform target)
        {
            _target = target;
            var newPosition = _target.position;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }

        private void Update()
        {
            if (_target == null) return;
            var newPosition= Vector3.Lerp(transform.position, _target.position,
                gameSettings.cameraSpeed * Time.deltaTime);
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
    }
}