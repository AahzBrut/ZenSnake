using System;
using Domain;
using Services;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class MazeController : MonoBehaviour
    {
        [SerializeField] private GrassController grassPrefab;

        private GameEventService _gameEventService;

        [Inject]
        public void Construct(
            GameEventService gameEventService
        )
        {
            _gameEventService = gameEventService;
            _gameEventService.MazeInitComplete += Init;
        }

        private void OnDestroy()
        {
            _gameEventService.MazeInitComplete -= Init;
        }

        private void Init(GameState gameState)
        {
            var grassController = Instantiate(grassPrefab, transform);
            grassController.Init(gameState.mazeSize);
        }
    }
}