using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackNSlash.Scripts.Enemy
{
    public class EnemyWaveManager : MonoBehaviour
    {
        public static EnemyWaveManager waveManagerInstance;
        
        [SerializeField] private EnemySpawner[] _enemySpawners;
        [SerializeField] private TextMeshProUGUI _waveText;
        public int _enemiesLeft;
        private int _currentWave = 0;

        private void Awake()
        {
            waveManagerInstance = this;
        }

        private void Update()
        {
            _waveText.text = "wave: " + _currentWave + "/4";
            
            if (_enemiesLeft <= 0)
            {
                _currentWave += 1;

                if (_currentWave == 5)
                {
                    // victory scene
                    SceneManager.LoadScene(3);
                }
                
                StartWave(_currentWave);
            }
        }

        public void StartWave(int waveIndex)
        {
            switch (waveIndex)
            {
                case 1:
                    _enemySpawners[0].SpawnEnemy();
                    break;
                case 2:
                    _enemySpawners[0].SpawnEnemy();
                    _enemySpawners[1].SpawnEnemy();
                    break;
                case 3:
                    _enemySpawners[0].SpawnEnemy();
                    _enemySpawners[1].SpawnEnemy();
                    _enemySpawners[2].SpawnEnemy();
                    break;
                case 4:
                    _enemySpawners[0].SpawnEnemy();
                    _enemySpawners[1].SpawnEnemy();
                    _enemySpawners[2].SpawnEnemy();
                    _enemySpawners[3].SpawnEnemy();
                    break;
            }
            
        }
    }
}