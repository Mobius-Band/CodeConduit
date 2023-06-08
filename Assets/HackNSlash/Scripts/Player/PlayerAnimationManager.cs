using System;
using UnityEngine;

namespace HackNSlash.Scripts.Player
{
    public class PlayerAnimationManager : MonoBehaviour
    {
        public Action OnAnimationEndCombo;
        public Action OnAnimationHit;
        public Action OnAnimationSuspendRotation;
        public Action OnAnimationReturningToIdle;
        public Action OnAnimationAttackStep;
        public Action OnAnimationEndDash;
        public Action OnAnimationSetNextAttack;
        public Action OnAnimationRegainMovement;
        public Action OnAnimationSuspendMovement;
        public Action OnAnimationMovementStep;

        void AnimationEndCombo()
        {
            OnAnimationEndCombo?.Invoke();
        }
    
        void AnimationHit()
        {
            OnAnimationHit?.Invoke();
        }
    
        void AnimationSuspendRotation()
        {
            OnAnimationSuspendRotation?.Invoke();
        }

        void AnimationReturningToIdle()
        {
            OnAnimationReturningToIdle?.Invoke();
        }

        void AnimationAttackStep()
        {
            OnAnimationAttackStep?.Invoke();
        }

        void AnimationEndDash()
        {
            OnAnimationEndDash?.Invoke();
        }

        void AnimationSetNextAttack()
        {
            OnAnimationSetNextAttack?.Invoke();
        }

        void AnimationRegainMovement()
        {
            OnAnimationRegainMovement?.Invoke();
        }
    
        void AnimationSuspendMovement()
        {
            OnAnimationSuspendMovement?.Invoke();
        }

        void AnimationMovementStep()
        {
            OnAnimationMovementStep?.Invoke();
        }
    }
}
