using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace HackNSlash.Scripts.Util
{
    public abstract class Tweener : MonoBehaviour
    {
        [SerializeField] protected float tweenDuration;
        [SerializeField] protected Ease easeType;
        [SerializeField] protected UnityEvent onTweenEnd;

        protected Sequence _tweenSequence;

        protected void Awake()
        {
            _tweenSequence = DOTween.Sequence();
            // _tweenSequence.onComplete += onTweenEnd.Invoke;
        }

        public abstract void TriggerTween();
    }
}