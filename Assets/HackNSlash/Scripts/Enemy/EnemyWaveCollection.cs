using System;
using UnityEngine;

namespace HackNSlash.Scripts.Enemy
{
    public class EnemyWaveCollection : MonoBehaviour
    {
        [SerializeField] private EnemyWave[] enemyWaves;

        public EnemyWave[] EnemyWaves => enemyWaves;
        public int Length => enemyWaves.Length;
        
        [ContextMenu("Gather Enemy Waves From Children")]
        public void GatherFromChild()
        {
            enemyWaves = GetComponentsInChildren<EnemyWave>();
        }
        
        [ContextMenu("Gather Enemy Waves AND Enemies From Children")]
        public void GatherFromChildRecursively()
        {
            enemyWaves = GetComponentsInChildren<EnemyWave>();
            Array.ForEach(enemyWaves, wave => wave.GatherFromChild());
        }
        
        
    }
}