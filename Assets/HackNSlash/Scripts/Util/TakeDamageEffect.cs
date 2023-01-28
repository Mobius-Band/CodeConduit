using System;
using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class TakeDamageEffect : MonoBehaviour 
    {
        [SerializeField] private Material _effectMaterial;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        [SerializeField] private float _speed;
        private Material _originalMaterial;

        private void Awake()
        {
            if (_meshRenderer != null)
            {
                _originalMaterial = _meshRenderer.material;
            }
            else
            {
                _originalMaterial = _skinnedMeshRenderer.material;
            }
        }

        public IEnumerator TakeDamageEffectCoroutine()
        {
            if (_meshRenderer != null)
            {
                _meshRenderer.material = _effectMaterial;
            }
            else
            {
                _skinnedMeshRenderer.material = _effectMaterial;
            }
            
            yield return new WaitForSeconds(_speed);
            if (_meshRenderer != null)
            {
                _meshRenderer.material = _originalMaterial;
            }
            else
            {
                _skinnedMeshRenderer.material = _originalMaterial;
            }
        }
    }
}
