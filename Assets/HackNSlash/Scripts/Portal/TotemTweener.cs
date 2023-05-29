using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace HackNSlash.Scripts.Portal
{
    public class TotemTweener : MonoBehaviour
    {
        [SerializeField] private float targetYPosition;
        [SerializeField] private Transform glowEffect;
        [SerializeField] private float targetEffectScale;
        [SerializeField] private float movementDuration;
        [SerializeField] private UnityEvent onTotemFullyTweened;
        
        private float originalYPosition;
        

        private void Awake()
        {
            originalYPosition = transform.localPosition.y;
        }

        public void TweenTowardsFinalPosition(int currentParcel, int maximumParcel)
        {
            float totalDistance = Mathf.Abs(originalYPosition - targetYPosition);
            float partDistance = totalDistance / maximumParcel;
            float nextTarget = transform.localPosition.y - (partDistance * currentParcel);
            Tween partTween = transform.DOLocalMoveY(nextTarget, movementDuration);
            if (currentParcel == maximumParcel)
            {
                partTween.onComplete += onTotemFullyTweened.Invoke;
            }
        }

        public void Tween()
        {
            var tweenSequence = DOTween.Sequence();
            tweenSequence.Append(transform.DOLocalMoveY(targetYPosition, movementDuration));
            tweenSequence.Join(glowEffect.DOScale(Vector3.one * targetEffectScale, movementDuration));
            tweenSequence.onComplete += onTotemFullyTweened.Invoke;
            tweenSequence.Play();
        }
    }
}