using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Tweener = HackNSlash.Scripts.Util.Tweener;

namespace HackNSlash.Scripts.UI.Credits
{
    public class ImageAlphaTweener : Tweener
    {
        [SerializeField] private Image image;
        [SerializeField] private float targetAlpha;

        public override void TriggerTween()
        {
            _tweenSequence = DOTween.Sequence();
            _tweenSequence.Join(image.DOFade(targetAlpha, tweenDuration));
            _tweenSequence.SetEase(easeType);
            _tweenSequence.Play();
            _tweenSequence.onComplete += onTweenEnd.Invoke;
        }
    }
}