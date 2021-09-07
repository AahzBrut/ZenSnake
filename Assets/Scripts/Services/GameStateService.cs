using System.Collections.Generic;
using Domain;
using Settings;
using UnityEngine;
using Utils;
using Zenject;

namespace Services
{
    public class GameStateService : MonoBehaviour
    {
        [SerializeField] private GameSettings gameSettings;
        private GameState _gameState;


        private GameEventService _gameEventService;
        private bool _isGamePaused = true;
        private bool _isGameOver;
        private bool _isGameRunning;
        
        [Inject]
        public void Construct(
            GameEventService gameEventService
        )
        {
            _gameEventService = gameEventService;
            _gameEventService.CollideWithApple += OnCollideWithApple;
            _gameEventService.CollideWithObstacle += OnCollideWithObstacle;
            _gameEventService.SpaceBarPressed += OnSpaceBarPressed;
        }

        private void OnSpaceBarPressed()
        {
            if (_isGameOver) return;
            _isGamePaused = !_isGamePaused;
            if (_isGamePaused)
            {
                _gameEventService.Pause.Invoke();
            }
            else
            {
                if (!_isGameRunning)
                {
                    _isGameRunning = true;
                    _gameEventService.GameStart.Invoke();
                }

                _gameEventService.UnPause.Invoke();
            }
        }

        private void OnCollideWithObstacle()
        {
            _isGameOver = true;
            _gameEventService.GameOver.Invoke();
        }

        private void OnCollideWithApple()
        {
            _gameState.score++;
            var head = _gameState.snake.snakeSegments[0];
            for (var i = _gameState.snake.snakeSegments.Keys.Count-1; i > 0; i--)
            {
                _gameState.snake.snakeSegments[i + 1] = _gameState.snake.snakeSegments[i];
                _gameState.snake.snakeSegments[i + 1].ordinalPosition = i + 1;
            }
            
            _gameState.snake.snakeSegments[1] = new SnakeSegmentData
            {
                position = head.position,
                ordinalPosition = 1,
                rotation = head.rotation,
                type = SnakeSegmentType.Body
            };
            
            _gameEventService.ScoreChanged.Invoke(_gameState.score);
            _gameEventService.AddSnakeSegment.Invoke(_gameState.snake.snakeSegments[1]);
        }

        private void OnDestroy()
        {
            _gameEventService.CollideWithApple -= OnCollideWithApple;
            _gameEventService.CollideWithObstacle -= OnCollideWithObstacle;
            _gameEventService.CollideWithObstacle -= OnSpaceBarPressed;
        }

        private void Start()
        {
            InitGameState();
            _gameEventService.MazeInitComplete.Invoke(_gameState);
            _gameEventService.ObstaclesInitComplete.Invoke(_gameState.obstacles);
            _gameEventService.SnakeInitComplete.Invoke(_gameState.snake);
            _gameEventService.ApplesInitComplete.Invoke(_gameState.apples);
            _gameEventService.GameInitCompleted.Invoke();
            _gameEventService.Pause.Invoke();
        }

        private void InitGameState()
        {
            _gameState = new GameState
            {
                mazeSize = gameSettings.mazeSize,
                numberOfApples = gameSettings.numberOfApples,
                numberOfObstacles = gameSettings.numberOfObstacles,
                snakeSpeed = gameSettings.snakeSpeed,
                snakeAngularSpeed = gameSettings.snakeAngularSpeed,
                obstacles = new List<ObstacleData>(),
                apples = new List<AppleData>(),
                snake = new SnakeData(),
                score = 0
            };
            InitObstacles();
            InitSnake();
            InitApples();
            _gameEventService.ScoreChanged.Invoke(_gameState.score);
        }

        private void InitApples()
        {
            for (var i = 0; i < _gameState.numberOfApples; i++)
            {
                var position = GetRandomPosition(GetAllObjectsPositions(), 1f);
                _gameState.apples.Add(new AppleData
                {
                    position = position,
                    state = AppleData.AppleState.Whole
                });
            } 
        }

        private void InitSnake()
        {
            var position = GetRandomPosition(GetAllObjectsPositions(), 4f);
            _gameState.snake.snakeSegments[0] = new SnakeSegmentData
            {
                ordinalPosition = 0,
                position = position,
                rotation = 180f,
                type = SnakeSegmentType.Head
            };
        }

        private List<Vector2> GetAllObjectsPositions()
        {
            var result = new List<Vector2>();
            foreach (var obstacle in _gameState.obstacles)
            {
                result.Add(obstacle.position);
            }

            foreach (var apple in _gameState.apples)
            {
                result.Add(apple.position);
            }

            foreach (var snake in _gameState.snake.snakeSegments.Values)
            {
                result.Add(snake.position);
            }
            return result;
        }

        private void InitObstacles()
        {
            var existingObstacles = new List<Vector2>();
            InitBorders();

            for (var i = 0; i < _gameState.numberOfObstacles; i++)
            {
                var position = GetRandomPosition(existingObstacles, 4f);
                var obstacle = new ObstacleData
                {
                    position = position,
                    size = Vector2.one
                };
                existingObstacles.Add(obstacle.position);
                _gameState.obstacles.Add(obstacle);
            }
        }

        private Vector2 GetRandomPosition(List<Vector2> existingObstacles, float minDistance)
        {
            var maxNumTries = 1000;
            Vector2 position;
            do
            {
                var cellX = Random.Range(0, _gameState.mazeSize.x);
                var cellY = Random.Range(0, _gameState.mazeSize.y);

                var x = cellX - _gameState.mazeSize.x / 2;
                var y = cellY - _gameState.mazeSize.y / 2;
                position = new Vector2(x, y);
            } while (!existingObstacles.IsFartherToAllThen(position, minDistance) && maxNumTries-- > 0);

            return position;
        }

        private void InitBorders()
        {
            InitHorizontalBorders();
            InitVerticalBorders();
        }

        private void InitVerticalBorders()
        {
            for (var y = 0; y < _gameState.mazeSize.y + 2; y++)
            {
                var obstacle = new ObstacleData
                {
                    position = new Vector2(-_gameState.mazeSize.x * .5f - .5f, y - _gameState.mazeSize.y * .5f - .5f),
                    size = Vector2.one
                };
                _gameState.obstacles.Add(obstacle);

                obstacle = new ObstacleData
                {
                    position = new Vector2(_gameState.mazeSize.x * .5f + .5f, y - _gameState.mazeSize.y * .5f - .5f),
                    size = Vector2.one
                };
                _gameState.obstacles.Add(obstacle);
            }
        }

        private void InitHorizontalBorders()
        {
            for (var x = 0; x < _gameState.mazeSize.x; x++)
            {
                var obstacle = new ObstacleData
                {
                    position = new Vector2(x - _gameState.mazeSize.x * .5f + .5f, -_gameState.mazeSize.y * .5f - .5f),
                    size = Vector2.one
                };
                _gameState.obstacles.Add(obstacle);

                obstacle = new ObstacleData
                {
                    position = new Vector2(x - _gameState.mazeSize.x * .5f + .5f, _gameState.mazeSize.y * .5f + .5f),
                    size = Vector2.one
                };
                _gameState.obstacles.Add(obstacle);
            }
        }
    }
}