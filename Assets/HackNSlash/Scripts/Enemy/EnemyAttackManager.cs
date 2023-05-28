using System;
using System.Collections;
using Combat;
using HackNSlash.Scripts.Enemy;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    [SerializeField] private EnemyAnimationManager _animationManager;
    [Header("PROJECTILE")]
    [SerializeField] private ProjectileAttack projectileAttack;
    [SerializeField] private LayerMask attackedMask;
    [Header("DASH")] 
    [SerializeField] private Hitbox hitbox;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float dashForce;
    [SerializeField] private int dashDamage;
    [SerializeField] private float velocityThreshold;

    private void Awake()
    {
        _animationManager.OnMouthAttack += Shoot;
    }

    //Should be called on an Animation Event
    public void Shoot()
    {
        projectileAttack.Execute(transform, attackedMask);
    }

    private IEnumerator CheckForMovementStability(Action action)
    {
        Debug.Log("Velocity: " + rigidbody.velocity.magnitude );
        if (rigidbody.velocity.sqrMagnitude > (velocityThreshold * velocityThreshold))
        {
            yield return null;
        }    
        action?.Invoke();
    }
    
    public void Dash()
    {
        rigidbody.AddForce(transform.forward * dashForce, ForceMode.Impulse);
        hitbox.damage = dashDamage;
        hitbox.StartTryHitOnce(transform);
        // StartCoroutine(CheckForMovementStability(hitbox.StopTryHitOnce));
    }

}
