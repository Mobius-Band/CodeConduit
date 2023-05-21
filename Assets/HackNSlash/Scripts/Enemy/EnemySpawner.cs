using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;
using VFXManager = HackNSlash.Scripts.VFX.VFXManager;

namespace HackNSlash.Scripts.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private VFXManager _vfxManager;
        
        public Action OnEnemySpawned;

        public GameObject SpawnEnemy(Transform enemyParent)
        {
            var newEnemy = Instantiate(_enemyPrefab, transform.position, quaternion.identity, enemyParent);
            OnEnemySpawned?.Invoke();
            newEnemy.SetActive(false);
            _vfxManager.visualEffects[0].visualEffect.gameObject.GetComponent<VisualEffect>().SetMesh("Mesh", newEnemy.GetComponentInChildren<MeshFilter>().mesh);
            _vfxManager.transform.localPosition = Vector3.zero;
            _vfxManager.transform.position = newEnemy.transform.position;
            _vfxManager.PlayVFX("spawn", transform);
            StartCoroutine(SetActiveWithDelay(newEnemy));
            
            return newEnemy;
        }

        private IEnumerator SetActiveWithDelay(GameObject enemy)
        {
            yield return new WaitForSeconds(2.5f);
            enemy.SetActive(true);
        }
    }
}
