using System.Collections;
using HackNSlash.Scripts.Util;
using HackNSlash.Scripts.VFX;
using UnityEngine;

namespace HackNSlash.Scripts.Enemy
{
    public class EnemyHealth : Health
    {
        [SerializeField] private VFXManager vfxManager;
        private Animator _animator;

        private void Awake()
        {
            base.Awake();
            _animator = GetComponentInChildren<Animator>();
        }
        
        protected override IEnumerator Die()
        {
            // _animator.SetBool("isFinalForm", false);
            // _animator.StopPlayback();
            
            // not working for some reason??
            _animator.Play("Enemy_DissolveOut");
            vfxManager.PlayVFX("dissolve", transform);
            // TODO: needs to stop enemy behaviour
            
            // yield return new WaitForSeconds(3.0f);
            
            Destroy(gameObject);
            yield break;
        }

        private void OnDestroy()
        {
            OnDeath?.Invoke();
        }
    }
}