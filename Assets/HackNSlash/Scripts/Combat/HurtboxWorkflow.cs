﻿using System;
using HackNSlash.Scripts.Audio;
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
            
        private void OnEnable()
        {
            if (health != null)
            {
                hurtbox.OnHitReceived += ctx => health.TakeDamage(ctx.Damage);
                if (_audioManager != null) hurtbox.OnHitReceived += ctx => _audioManager.PlayRandom("dmg");
            }

            if (knockback != null)
            {
                if (_animator != null)
                {
                    hurtbox.OnHitReceived += ctx => _animator.Play("Hit", 0);
                }
                hurtbox.OnHitReceived += ctx => knockback.ApplyKnockback(ctx.hitOriginTransform, ctx.Damage);
            }
        }
    }
}