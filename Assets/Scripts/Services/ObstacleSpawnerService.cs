using System;
using System.Collections.Generic;
using Controllers;
using Domain;
using UnityEngine;
using Zenject;

namespace Services
{
    public class ObstacleSpawnerService : MonoBehaviour
    {
        [SerializeField] private ObstacleController obstaclePrefab;

        private GameEventService _gameEventService;
        
        [Inject]
        public void Construct(GameEventService gameEventService)
        {
            _gameEventService = gameEventService;
            _gameEventService.ObstaclesInitComplete += Init;
        }

        private void OnDestroy()
        {
            _gameEventService.ObstaclesInitComplete -= Init;
        }

        private void Init(List<ObstacleData> obstacles)
        {
            foreach (var obstacleData in obstacles)
            {
                var obstacle = Instantiate(obstaclePrefab);
                obstacle.Init(obstacleData);
            }
        }
    }
}