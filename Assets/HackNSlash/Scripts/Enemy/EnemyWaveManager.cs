using System.Collections;
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
        [SerializeField] private UnityEvent<int, int> OnWaveFinished;
        [SerializeField] private UnityEvent OnAllWavesFinished;
        [Space(10)] 
        [SerializeField] private bool startWithLastWave;
        
        private int _enemiesLeft => _enemyParent.childCount;
        private int _currentWave = 0;
        
        private void Start()
        {
            _currentWave = startWithLastWave ? _maximumWave : 1;
            StartWave(_currentWave);
        }

        private void CheckWaveProgression()
        {
            if (_enemiesLeft > 0)
            {
                return;
            }
            OnWaveFinished?.Invoke(_currentWave, _maximumWave);
            _currentWave += 1;
                
            if (_currentWave > _maximumWave)
            {
                OnAllWavesFinished?.Invoke();
                GameManager.Instance.UnlockCurrentLaserWall();
                enabled = false;
                return;
            }
                
            if (!enabled) return;
                
            StartWave(_currentWave);
        }
        
        public void StartWave(int waveIndex)
        {
            for (int i = 0; i < waveIndex; i++)
            {
                var enemy = _enemySpawners[i].SpawnEnemy(_enemyParent);
                enemy.GetComponent<EnemyHealth>().OnDeath += EnemyDied;
                enemy.GetComponent<EnemyBehaviours>().attackTarget = playerTransform;
            }
            _waveText.text = $"wave: {_currentWave}/{_maximumWave}";
        }
        
        private IEnumerator LateEnemyDied()
        {
            yield return null;
            CheckWaveProgression();
        }
        private void EnemyDied() => StartCoroutine(LateEnemyDied());
    }
}
