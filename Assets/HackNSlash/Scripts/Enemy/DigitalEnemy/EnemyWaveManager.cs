using System;
using System.Collections;
using HackNSlash.Scripts.GameManagement;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace HackNSlash.Scripts.Enemy
{
    public class EnemyWaveManager : MonoBehaviour
    {
        [SerializeField] private EnemyWaveCollection selectedWaveCollection;
        [SerializeField] private Transform _enemyParent;
        [SerializeField] private TextMeshProUGUI _waveText;
        [SerializeField] private Transform playerTransform;
        [FormerlySerializedAs("OnWaveFinished")]
        [Space] 
        [SerializeField] private UnityEvent<int, int> onWaveFinished;
        [FormerlySerializedAs("OnAllWavesFinished")] [SerializeField] private UnityEvent onAllWavesFinished;
        
        private int _currentWaveIndex = 0;
        private int _enemiesLeft => _enemyParent.childCount;
        private int MaximumWaveIndex => selectedWaveCollection.Length;

        public UnityEvent OnAllWavesFinished => onAllWavesFinished;
        
        public void Initialize()
        {
            StartWave();
        }

        public void StartWave(int waveIndex)
        {
            EnemyWave[] enemyWaves = selectedWaveCollection.EnemyWaves;
            EnemyWave wave = enemyWaves[waveIndex];
            wave.SpawnEnemies(_enemyParent, playerTransform, EnemyDied);

            if (_waveText != null)
            {
                _waveText.text = $"wave: {_currentWaveIndex}/{enemyWaves.Length}";                
            }
        }
        
        public void StartWave() => StartWave(_currentWaveIndex);

        
        private void CheckWaveProgression()
        {
            if (_enemiesLeft > 0)
            {
                return;
            }

            onWaveFinished?.Invoke(_currentWaveIndex,MaximumWaveIndex);
            _currentWaveIndex += 1;
                
            if (_currentWaveIndex >= MaximumWaveIndex)
            {
                onAllWavesFinished?.Invoke();
                GameManager.Instance.UnlockCurrentLaserWall();
                enabled = false;
                return;
            }
                
            if (!enabled) return;
                
            StartWave();
        }
        
        private IEnumerator LateEnemyDied()
        {
            yield return null;
            if (!gameObject.IsDestroyed())
            {
                CheckWaveProgression();
            }
        }
        private void EnemyDied() => StartCoroutine(LateEnemyDied());

        [ContextMenu("Set Enemy Waves Up")]
        public EnemyWaveCollection[] SetEnemyWavesUp()
        {
            var enemyWavesCollection = GetComponentsInChildren<EnemyWaveCollection>();
            foreach (var waveCollection in enemyWavesCollection)
            {
                waveCollection.GatherFromChildRecursively();
            }

            return enemyWavesCollection;
        }

        private void OnValidate()
        {
            SetEnemyWavesUp();
        }
    }
}
