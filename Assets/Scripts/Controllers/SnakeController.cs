using System.Collections.Generic;
using Domain;
using Services;
using Settings;
using UnityEngine;
using Utils;
using Zenject;

namespace Controllers
{
    public class SnakeController : MonoBehaviour
    {
        [SerializeField] private GameSettings gameSettings;
        
        private SnakeSegmentData _data;
        private Transform _target;
        private SnakeController _parentSegment;
        private Vector2 _currentTargetPosition;

        private readonly Queue<Vector2> _positions = new Queue<Vector2>();
        private GameEventService _gameEventService;
        private bool _stopped;

        [Inject]
        public void Construct(GameEventService gameEventService)
        {
            _gameEventService = gameEventService;
            _gameEventService.GameOver += OnSnakeDie;
            _gameEventService.Pause += OnPause;
            _gameEventService.UnPause += OnUnPause;
        }

        private void OnUnPause()
        {
            _stopped = false;
        }

        private void OnPause()
        {
            _stopped = true;
        }

        private void OnSnakeDie()
        {
            _stopped = true;
        }

        private void OnDestroy()
        {
            _gameEventService.GameOver -= OnSnakeDie;
            _gameEventService.GameOver -= OnPause;
        }

        public void Init(SnakeSegmentData data)
        {
            _data = data;
            transform.position = _data.position;
            transform.rotation = Quaternion.AngleAxis(_data.rotation, Vector3.forward);
            _currentTargetPosition = gameSettings.mazeSize;
            _positions.Enqueue(transform.position);
        }

        public void SetTarget(Transform target)
        {
            _target = target;
            var parentSegment = target.GetComponent<SnakeController>();
            if (parentSegment != null)
            {
                _parentSegment = parentSegment;
                _parentSegment._positions.Clear();
            }
        }

        private void Update()
        {
            if (_stopped) return;
            
            if (_data.type == SnakeSegmentType.Head)
            {
                MoveToTarget();
                RotateToTarget();
            }
            else
            {
                FollowTarget();
            }
            _positions.Enqueue(transform.position);
            SyncData();
        }

        private void RotateToTarget()
        {
            var vectorToTarget = _target.position - transform.position;
            var targetAngle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            var targetRotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);
            transform.rotation =
                Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * gameSettings.snakeAngularSpeed);
        }

        private void MoveToTarget()
        {
            var currentPosition = transform.position;
            
            currentPosition = Vector3.Lerp(currentPosition,
                currentPosition + transform.right * gameSettings.snakeSpeed,
                Time.deltaTime * gameSettings.snakeAngularSpeed);
            
            transform.position = currentPosition;
        }

        private void FollowTarget()
        {
            if (_currentTargetPosition == gameSettings.mazeSize.ToVector2())
            {
                _currentTargetPosition = _parentSegment._positions.Dequeue();
            }

            if (Vector2.Distance(_parentSegment.transform.position, _currentTargetPosition) >= gameSettings.snakeSegmentSize)
            {
                transform.position = _currentTargetPosition;
                _currentTargetPosition = _parentSegment._positions.Dequeue();
                while (Vector2.Distance(_parentSegment.transform.position, _currentTargetPosition) >= gameSettings.snakeSegmentSize)
                {
                    transform.position = _currentTargetPosition;
                    _currentTargetPosition = _parentSegment._positions.Dequeue();
                }
            }

            var distanceToParent = Vector2.Distance(_parentSegment.transform.position, transform.position);
            var targetPosDirection = (Vector3)_currentTargetPosition - transform.position;
            if (distanceToParent > gameSettings.snakeSegmentSize)
            {
                if (targetPosDirection.magnitude > distanceToParent - gameSettings.snakeSegmentSize)
                {
                    transform.position += targetPosDirection * (distanceToParent - gameSettings.snakeSegmentSize);
                }
            }
        }

        private void SyncData()
        {
            _data.position = transform.position;
            transform.rotation.ToAngleAxis(out var currentRotation, out var axis);
            _data.rotation = currentRotation;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Apples"))
            {
                var apple = other.gameObject.GetComponent<AppleController>();
                if (apple != null && !apple.IsEaten)
                {
                    apple.Eat();
                    _gameEventService.CollideWithApple.Invoke();
                }
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
            {
                _gameEventService.CollideWithObstacle.Invoke();
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("SnakeBody"))
            {
                var snakeController = other.gameObject.GetComponent<SnakeController>();
                if (snakeController._target != transform)
                {
                    _gameEventService.CollideWithObstacle.Invoke();
                }
            }
        }
    }
}