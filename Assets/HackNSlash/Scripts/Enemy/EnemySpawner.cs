using System;
using System.Collections;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;
using VFXManager = HackNSlash.Scripts.VFX.VFXManager;

namespace HackNSlash.Scripts.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _enemyPrefab;
        private VFXManager _vfxManager;
        
        public Action OnEnemySpawned;

        public GameObject SpawnEnemy(Transform enemyParent)
        {
            var newEnemy = Instantiate(_enemyPrefab, transform.position, quaternion.identity, enemyParent);
            OnEnemySpawned?.Invoke();
            newEnemy.SetActive(false);
            // newEnemy.GetComponentInChildren<MeshRenderer>().enabled = false;
            _vfxManager = newEnemy.transform.GetComponentInChildren<VFXManager>();
            if (_vfxManager != null)
            {
                // _vfxManager.visualEffects[0].visualEffect.GetComponentInChildren<VisualEffect>().SetMesh("Mesh", newEnemy.GetComponentInChildren<MeshFilter>().mesh);
                var vfx = _vfxManager.visualEffects[0];
                if (vfx == null)
                {
                    Debug.LogWarning($"VFX is null");
                }

                var vfxGO = _vfxManager.visualEffects;
                if (vfxGO == null)
                {
                    Debug.LogWarning($"vfxGO is null");
                }
                _vfxManager.transform.localPosition = Vector3.zero;
                _vfxManager.transform.position = newEnemy.transform.position;
                newEnemy.SetActive(true);
                _vfxManager.PlayVFX("spawn", transform);
            }
            else
            {
                Debug.LogWarning($"{name}'s VFXManager couldn't be found");
            }

            StartCoroutine(SetActiveWithDelay(newEnemy));
            
            return newEnemy;
        }

        public GameObject SpawnEnemy(Transform enemyParent, Transform primaryTarget, Action deathAction)
        {
            var enemy = SpawnEnemy(enemyParent);
            enemy.GetComponent<EnemyHealth>().OnDeath += deathAction;
            enemy.GetComponent<EnemyBehaviours>().attackTarget = primaryTarget;
            return enemy;
        }

        private IEnumerator SetActiveWithDelay(GameObject enemy)
        {
            yield return new WaitForSeconds(2.5f);
            enemy.SetActive(true);
        }

        public void DrawGizmo()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1);
        }

        private void OnDrawGizmosSelected()
        {
            DrawGizmo();
        }
    }
}
