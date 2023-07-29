using System.Collections;
using HackNSlash.Scripts.Util;
using HackNSlash.Scripts.VFX;
using UnityEngine;

namespace HackNSlash.Scripts.Enemy
{
    public class EnemyHealth : Health
    {
        [SerializeField] private VFXManager vfxManager;
        [SerializeField] private EnemyAnimationManager animationManager;
        private Animator _animator;

        private new void Awake()
        {
            base.Awake();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            animationManager.OnDeathEnd.AddListener(() => Destroy(gameObject));
        }
        
        protected override IEnumerator Die()
        {
            _animator.SetTrigger("die");
            vfxManager.PlayVFX("dissolve", transform);
            yield break;
        }

        private void OnDestroy()
        {
            OnDeath?.Invoke();
        }
    }
}