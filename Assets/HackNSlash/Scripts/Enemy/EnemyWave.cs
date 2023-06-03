using System;
using UnityEngine;

namespace HackNSlash.Scripts.Enemy
{
    public class EnemyWave : MonoBehaviour
    {
        [SerializeField] private EnemySpawner[] _enemySpawners;
        [SerializeField] private bool doAlwaysDrawGizmos;
        public void SpawnEnemies(Transform enemyParent, Transform primaryTarget, Action deathAction)
        {
            for (int i = 0; i < _enemySpawners.Length; i++)
            {
                _enemySpawners[i].SpawnEnemy(enemyParent, primaryTarget, deathAction);
            }
        }
        
        [ContextMenu("Gather From Children")]
        public void GatherFromChild()
        {
            _enemySpawners = GetComponentsInChildren<EnemySpawner>();
        }

        private void OnDrawGizmosSelected()
        {
            Array.ForEach(_enemySpawners, e => e.DrawGizmo());
        }

        private void OnDrawGizmos()
        {
            if (!doAlwaysDrawGizmos)
            {
                return;
            }
            Array.ForEach(_enemySpawners, e => e.DrawGizmo());
        }
    }
}