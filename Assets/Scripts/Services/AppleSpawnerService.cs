using System.Collections.Generic;
using Controllers;
using Domain;
using UnityEngine;
using Zenject;

namespace Services
{
    public class AppleSpawnerService : MonoBehaviour
    {
        [SerializeField] private AppleController applePrefab;

        private GameEventService _gameEventService;
        
        [Inject]
        public void Construct(GameEventService gameEventService)
        {
            _gameEventService = gameEventService;
            _gameEventService.ApplesInitComplete += Init;
        }

        private void OnDestroy()
        {
            _gameEventService.ApplesInitComplete -= Init;
        }

        private void Init(List<AppleData> apples)
        {
            foreach (var appleData in apples)
            {
                var apple = Instantiate(applePrefab, appleData.position, Quaternion.identity);
                apple.Init(appleData);
            }
        }
    }
}