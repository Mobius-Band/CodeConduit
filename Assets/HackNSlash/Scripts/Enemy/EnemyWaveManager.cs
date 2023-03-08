using System;
using HackNSlash.Scripts.GameManagement;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace HackNSlash.Scripts.Enemy
{
    public class EnemyWaveManager : MonoBehaviour
    {
        [SerializeField] private EnemySpawner[] _enemySpawners;
        [SerializeField] private Transform _enemyParent;
        [SerializeField] private int _maximumWave;
        [SerializeField] private TextMeshProUGUI _waveText;
        [SerializeField] private Transform playerTransform;
        [Space] 
        [SerializeField] private UnityEvent OnWaveFinished;
        [SerializeField] private UnityEvent OnAllWavesFinished;
        [Space(10)] 
        [SerializeField] private bool startWithLastWave;
        
        private int _enemiesLeft;
        private int _currentWave = 0;

        private void Awake()
        {
            SetSpawningCount();

            if (startWithLastWave)
            {
                _currentWave = _maximumWave-1;
            }
        }

        private void Update()
        {
            _waveText.text = $"wave: {_currentWave}/{_maximumWave}";
            
            if (_enemiesLeft <= 0)
            {
                _currentWave += 1;
                
                if (_currentWave > _maximumWave)
                {
                    OnAllWavesFinished?.Invoke();
                    GameManager.Instance.UnlockCurrentLaserWall();
                    enabled = false;
                }
                
                if (!enabled) return;
                
                OnWaveFinished?.Invoke();
                StartWave(_currentWave);
            }
        }
        
        public void StartWave(int waveIndex)
        {
            for (int i = 0; i < waveIndex; i++)
            {
                var enemy = _enemySpawners[i].SpawnEnemy(_enemyParent);
                enemy.GetComponent<EnemyHealth>().OnDeath += EnemyDied;
                enemy.GetComponent<EnemyBehaviour>().target = playerTransform;
            }
        }
        
        private void EnemyDied()
        {
            _enemiesLeft--;
            Debug.Log("Enemies Left: " + _enemiesLeft);

        }
        
        private void EnemySpawned()
        {
            _enemiesLeft++;
            Debug.Log("Enemies Left: " + _enemiesLeft);

        }

        private void SetSpawningCount()
        {
            Array.ForEach(_enemySpawners, ctx => ctx.OnEnemySpawned += EnemySpawned);
        }
    }
}
