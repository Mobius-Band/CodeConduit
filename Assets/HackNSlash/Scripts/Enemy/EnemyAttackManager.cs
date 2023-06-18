using System;
using System.Collections;
using Combat;
using HackNSlash.Scripts.Audio;
using HackNSlash.Scripts.Enemy;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    [SerializeField] private EnemyAnimationManager _animationManager;
    [SerializeField] private AudioManager _audioManager;
    [Header("PROJECTILE")]
    [SerializeField] private ProjectileAttack projectileAttack;
    [SerializeField] private LayerMask attackedMask;
    [Header("DASH")] 
    [SerializeField] private Hitbox hitbox;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float dashForce;
    [SerializeField] private int dashDamage;
    [SerializeField] private float velocityThreshold;
    [SerializeField] private float preparationTime;

    public float DashPreparationTime => preparationTime;

    private void Start()
    {
        _animationManager.OnMouthAttack += Shoot;
        _animationManager.OnMouthAttackBegin.AddListener(() => _audioManager.PlayRandom("projectileAtk"));
    }

    //Should be called on an Animation Event
    public void Shoot()
    {
        projectileAttack.Execute(transform, attackedMask);
    }

    private IEnumerator CheckForMovementStability(Action action)
    {
        while (rigidbody.velocity.sqrMagnitude > (velocityThreshold * velocityThreshold))
        {
            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }    
        action?.Invoke();
    }
    
    public void Dash()
    {
        rigidbody.AddForce(transform.forward * dashForce, ForceMode.Impulse);
        hitbox.damage = dashDamage;
        hitbox.StartTryHitOnce(transform);
        _audioManager.PlayRandom("dash");
        StartCoroutine(CheckForMovementStability(hitbox.StopTryHitOnce));
    }

}
