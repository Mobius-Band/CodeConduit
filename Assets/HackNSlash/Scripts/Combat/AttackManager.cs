using Unity.VisualScripting;
using UnityEngine;

namespace Combat
{
    /// <summary>
    /// This script is responsible for managing a character`s attacks, using a reference to its hitbox and an array
    /// of serialized attacks
    /// </summary>
    public class AttackManager : MonoBehaviour
    {
        [Tooltip("A collection of attacks that can be used by this character.")]
        public MeleeAttack[] attacks; 
        [Tooltip("The hitbox that will be used to detect collisions with other objects - this should be a child of the character")]
        [SerializeField] private Hitbox hitbox;
        [Tooltip("An integer that represents the index of the current meleeAttack in the attacks array" +
                 "\n In editor: Click the three dots in the upper right corner of this component and " +
                 "press 'Set Current MeleeAttack' to visualize the current meleeAttack´s hitbox")]
        public int currentAttackIndex = 0;
        
        public MeleeAttack CurrentMeleeAttack => attacks[currentAttackIndex];

        [HideInInspector] public bool _isAttacking;
        public Animator animator;

        [Tooltip("Apply current meleeAttack's position and size to the hitbox, for debugging. " +
                 "Use this to visualize the current meleeAttack's hitbox in the editor.")]
        [ContextMenu("Set Current MeleeAttack")]
        private void SetCurrentAttack()
        {
            if (hitbox.IsUnityNull())
            {
                hitbox = GetComponent<Hitbox>();
            }

            if (currentAttackIndex < attacks.Length || currentAttackIndex >= 0)
            {
                hitbox.SetValues(CurrentMeleeAttack);
            }
        }
        
        /// <summary>
        ///Automatically sets index and hitbox properties
        /// </summary>
        private void SetCurrentAttack(int index)
        {
            currentAttackIndex = index;
            SetCurrentAttack();
        }
        
        /// <summary>
        /// Called by an Animation Event to start the current meleeAttack 
        /// </summary>
        public void ToggleHitbox()
        {
            hitbox.TryHit(transform);
        }
        
        /// <summary>
        ///  Set index for current meleeAttack and trigger it´s animation
        /// </summary>
        /// <param name="index"> Index of the meleeAttack in the attacks array</param>
        public void Attack(int index)
        {
            _isAttacking = true;
            animator.SetTrigger("goToNextAttackAnimation");
            SetCurrentAttack(index);
        }
        
        private void OnValidate()
        {
            SetCurrentAttack();
        }

        public void StopAttack()
        {
            _isAttacking = false;
        }
    }
}