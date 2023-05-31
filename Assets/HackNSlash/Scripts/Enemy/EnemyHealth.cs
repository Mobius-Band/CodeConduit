using System;
using HackNSlash.Scripts.Util;
using HackNSlash.Scripts.VFX;
using UnityEngine;
using Util;

namespace HackNSlash.Scripts.Enemy
{
    public class EnemyHealth : Health
    {
        [SerializeField] private VFXManager _vfxManager;
        
        protected override void Die()
        {
            //TODO: Should be handled solely by score or a bridging class
            // Score.scoreInstance.AddAmount(10);
            _vfxManager.PlayVFX("spawn", transform);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            OnDeath?.Invoke();
        }
    }
}