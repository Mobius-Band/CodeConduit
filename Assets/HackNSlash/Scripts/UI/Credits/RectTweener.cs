using System;
using DG.Tweening;
using UnityEngine;
using Tweener = HackNSlash.Scripts.Util.Tweener;

namespace HackNSlash.Scripts.UI.Credits
{
    public class RectTweener : Tweener
    {
        [SerializeField] private Vector3 targetPosition;
        [SerializeField] private RectTransform rect;

        public override void TriggerTween()
        {
            _tweenSequence.Join(rect.DOAnchorPos(targetPosition, tweenDuration));
            _tweenSequence.Play();
            _tweenSequence.onComplete += onTweenEnd.Invoke;
        }
    }
}