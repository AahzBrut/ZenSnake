using System.Collections.Generic;
using Controllers;
using Domain;
using Settings;
using UnityEngine;
using Zenject;

namespace Services
{
    public class SnakeSpawnerService : MonoBehaviour
    {
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private SnakeController headPrefab;
        [SerializeField] private SnakeController bodyPrefab;

        private readonly List<SnakeController> _snakeSegments = new List<SnakeController>();

        private MouseTargetController _mouseTarget;
        private GameEventService _gameEventService;

        [Inject]
        public void Construct(
            MouseTargetController mouseTarget,
            GameEventService gameEventService
        )
        {
            _gameEventService = gameEventService;
            _gameEventService.SnakeInitComplete += Init;
            _gameEventService.AddSnakeSegment += OnAddSnakeSegment;
            _mouseTarget = mouseTarget;
        }

        private void OnAddSnakeSegment(SnakeSegmentData data)
        {
            var newSegment = Instantiate(bodyPrefab, data.position,
                Quaternion.AngleAxis(data.rotation, Vector3.forward));
            
            _snakeSegments.Insert(1, newSegment);
            newSegment.Init(data);
            newSegment.SetTarget(_snakeSegments[0].transform);
            if (_snakeSegments.Count > 2)
            {
                _snakeSegments[2].SetTarget(_snakeSegments[1].transform);
            }
        }

        private void OnDestroy()
        {
            _gameEventService.SnakeInitComplete -= Init;
            _gameEventService.AddSnakeSegment += OnAddSnakeSegment;
        }

        private void Init(SnakeData data)
        {
            foreach (var segmentData in data.snakeSegments.Values)
            {
                var prefab = segmentData.type == SnakeSegmentType.Head ? headPrefab : bodyPrefab;
                var segment = Instantiate(prefab, segmentData.position,
                    Quaternion.AngleAxis(segmentData.rotation, Vector3.forward));
                segment.Init(segmentData);
                segment.SetTarget(segmentData.type == SnakeSegmentType.Head
                    ? _mouseTarget.transform
                    : _snakeSegments[_snakeSegments.Count - 1].transform);
                _snakeSegments.Add(segment);
            }
            _gameEventService.TargetAcquired.Invoke(_snakeSegments[0].transform);
        }
    }
}