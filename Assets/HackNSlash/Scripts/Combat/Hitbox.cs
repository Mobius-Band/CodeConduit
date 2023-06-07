using System;
using System.Collections;
using Ez;
using Ez.Msg.Demos;
using HackNSlash.Scripts.Audio;
using Unity.VisualScripting;
using UnityEngine;

namespace Combat
{
    public enum ColliderState
    {
        Inactive,
        Active,
        Hit
    }
    
    /// <summary>
    /// This class is responsible for handling the collision of the character's weapon with the enemy.
    ///  It is attached to a dedicated child of the character. Should be used in the AttackManager component.
    /// </summary>
    public class Hitbox : MonoBehaviour
    {
        public LayerMask mask;
        [Header("Debugging Colors")]
        public Color inactiveColor = Color.gray;
        public Color activeColor = Color.blue;
        public Color hitColor = Color.red;
        [HideInInspector] public int damage;
        [HideInInspector] public int knockback;

        
        private ColliderState _state = ColliderState.Inactive;
        private Collider[] _hitColliders = {};

        private Coroutine _hitOnceRoutine;

        private Color StateColor =>
            _state switch
            {
                ColliderState.Inactive => inactiveColor,
                ColliderState.Active => activeColor,
                ColliderState.Hit => hitColor,
                _ => Color.white
            };

        [SerializeField] private AudioManager _audioManager;
        
        /// <summary>
        ///  Uses the hitbox data to check for colliders and apply damage to them.
        /// </summary>
        // public bool TryHit(Transform attackerTransform)
        // {
        //     return TryHit(attackerTransform, damage, mask);
        // }

        public bool TryHit(Transform attackerTransform)
        {
            _hitColliders = Physics.OverlapBox(
                transform.position, transform.localScale / 2, transform.localRotation, mask);
            if (_hitColliders.Length <= 0) 
                return false;
            var hitEventArgs = new HitEventArgs()
            {
                Damage = damage,
                Knockback = knockback,
                hitOriginTransform = attackerTransform
            };
            Array.ForEach(_hitColliders, 
                hitCollider => hitCollider.gameObject.Send<IHitResponder>(_=>_.HitRespond(hitEventArgs)));
            if (_audioManager != null) _audioManager.PlayRandom("hit");
            return true;
        }

        private IEnumerator TryHitOnceCoroutine(Transform a)
        {
            while (!TryHit(a))
            {
                yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
            }
        }
        
        public void StartTryHitOnce(Transform attackerTransform)
        {
            StopTryHitOnce();
            if (_hitOnceRoutine == null)
            {
                _hitOnceRoutine = StartCoroutine(TryHitOnceCoroutine(attackerTransform));
            }
        }

        public void StopTryHitOnce()
        {
            if (_hitOnceRoutine != null)
            {
                StopCoroutine(_hitOnceRoutine);
                _hitOnceRoutine = null;
            }
        }
        
        /// <summary>
        /// Sets hitbox data
        /// </summary>
        public void SetValues(Vector3 localPosition, Vector3 dimensions, int damage, int knockback)
        {
            transform.localPosition = localPosition;
            transform.localScale = dimensions;
            this.damage = damage;
            this.knockback = knockback;
        }
        
        /// <summary>
        /// Sets hitbox data, using an attack data object;
        /// </summary>
        public void SetValues(Attack attack)
        {
            SetValues(attack.hitboxPosition, attack.hitboxSize, attack.damage, attack.knockbackAmount);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = StateColor;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, transform.localScale);
        }
    }
}