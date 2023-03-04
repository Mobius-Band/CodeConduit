using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public Action OnAnimationEndCombo;
    public Action OnAnimationHit;
    public Action OnAnimationSuspendRotation;
    public Action OnAnimationReturningToIdle;
    public Action OnAnimationAttackStep;
    public Action OnAnimationEndDash;
    public Action OnAnimationSetNextAttack;

    
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
}
