using Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI startGame;
        [SerializeField] private TextMeshProUGUI pauseGame;
        [SerializeField] private TextMeshProUGUI gameOver;

        private GameEventService _gameEventService;
        private bool _isGameRunning;
        
        [Inject]
        public void Construct(GameEventService gameEventService)
        {
            _gameEventService = gameEventService;
            _gameEventService.ScoreChanged += OnScoreChanged;
            _gameEventService.GameStart += OnGameStart;
            _gameEventService.GameOver += OnGameOver;
            _gameEventService.GameInitCompleted += OnGameInitCompleted;
            _gameEventService.Pause += OnGamePaused;
            _gameEventService.UnPause += OnGameUnPaused;
        }

        private void OnGameUnPaused()
        {
            pauseGame.gameObject.SetActive(false);
        }

        private void OnGamePaused()
        {
            if (_isGameRunning)
            {
                pauseGame.gameObject.SetActive(true);
            }
        }

        private void OnGameInitCompleted()
        {
            startGame.gameObject.SetActive(true);
        }

        private void OnGameOver()
        {
            gameOver.gameObject.SetActive(true);
        }

        private void OnGameStart()
        {
            _isGameRunning = true;
            startGame.gameObject.SetActive(false);
        }

        private void OnScoreChanged(int score)
        {
            scoreText.text = $"Score: {score,4:D4}";
        }

        private void OnDestroy()
        {
            _gameEventService.ScoreChanged -= OnScoreChanged;
            _gameEventService.GameStart -= OnGameStart;
            _gameEventService.GameOver -= OnGameOver;
            _gameEventService.GameInitCompleted -= OnGameInitCompleted;
            _gameEventService.Pause -= OnGamePaused;
            _gameEventService.UnPause -= OnGameUnPaused;
        }
    }
}