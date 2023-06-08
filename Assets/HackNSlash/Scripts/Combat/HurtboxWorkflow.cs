using System;
using HackNSlash.Scripts.Audio;
using HackNSlash.Scripts.Combat;
using HackNSlash.Scripts.Util;
using HackNSlash.Scripts.VFX;
using UnityEngine;
using Util;

namespace Combat
{
    /// <summary>
    /// This class is responsible for connecting health and hurtbox for damage.
    /// </summary>
    public class HurtboxWorkflow : MonoBehaviour
    {
        [SerializeField] private Hurtbox hurtbox;
        [SerializeField] private Health health;
        [SerializeField] private Knockback knockback;
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private VFXManager _vfxManager;
        
        private void OnEnable()
        {
            if (health != null)
            {
                hurtbox.OnHitReceived += ctx => health.TakeDamage(ctx.Damage);
                if (_audioManager != null) hurtbox.OnHitReceived += ctx => _audioManager.PlayRandom("damage");
                if (_vfxManager != null) hurtbox.OnHitReceived += ctx => _vfxManager.PlayVFX("impact", transform);
            }

            if (knockback != null)
            {
                if (_animator != null)
                {
                    hurtbox.OnHitReceived += ctx => _animator.Play("Hit", 0);
                }
                hurtbox.OnHitReceived += ctx => knockback.ApplyKnockback(ctx.hitOriginTransform, ctx.Knockback);
            }
        }
    }
}