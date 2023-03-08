using System;
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

        
        private ColliderState _state = ColliderState.Inactive;
        private Collider[] _hitColliders = {};
        
        private Vector3 position => transform.position;

        private Color StateColor =>
            _state switch
            {
                ColliderState.Inactive => inactiveColor,
                ColliderState.Active => activeColor,
                ColliderState.Hit => hitColor,
                _ => Color.white
            };

        [SerializeField] private AudioManager _audioManager;

        void Update()
        {
            if (_state == ColliderState.Inactive)
            {
                return;
            }
            TryHitUpdate();
        }


        
        /// <summary>
        ///  Uses the hitbox data to check for colliders and apply damage to them.
        /// </summary>
        public void TryHit(Transform attackerTransform)
        {
            _hitColliders = Physics.OverlapBox(
                transform.position, transform.localScale / 2, transform.localRotation, mask);
            if (_hitColliders.Length <= 0) 
                return;
            var hitEventArgs = new HitEventArgs()
            {
                Damage = damage,
                hitOriginTransform = attackerTransform
            };
            Array.ForEach(_hitColliders, 
                hitCollider => hitCollider.gameObject.Send<IHitResponder>(_=>_.HitRespond(hitEventArgs)));
            if (_audioManager != null) _audioManager.PlayRandom("hit", false);
        }
        
        /// <summary>
        ///   Uses the hitbox data to check for colliders and apply damage to them. Continuously check for collider.
        ///    Should be used for continuous damage. Should be used in Update.
        /// </summary>
        private void TryHitUpdate()
        {
            Physics.OverlapBoxNonAlloc(transform.position, transform.localScale/2, _hitColliders, transform.localRotation, mask);
            if (_hitColliders.Length > 0)
            {
                _state = ColliderState.Hit;
                //TODO Put damage response here
            }
            else
            {
                _state = ColliderState.Active;
            }
        }
        
        /// <summary>
        /// Sets hitbox data
        /// </summary>
        public void SetValues(Vector3 localPosition, Vector3 dimensions, int damage)
        {
            transform.localPosition = localPosition;
            transform.localScale = dimensions;
            this.damage = damage;
        }
        
        /// <summary>
        /// Sets hitbox data, using an attack data object;
        /// </summary>
        public void SetValues(Attack attack)
        {
            SetValues(attack.hitboxPosition, attack.hitboxSize, attack.damage);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = StateColor;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, transform.localScale);
        }
    }
}