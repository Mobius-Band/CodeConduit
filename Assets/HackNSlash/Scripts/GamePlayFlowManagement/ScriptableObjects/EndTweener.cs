using System;
using DG.Tweening;
using HackNSlash.Scripts.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HackNSlash.Scripts.GamePlayFlowManagement
{
    public class EndTweener : MonoBehaviour
    {
        [SerializeField] private UnityEvent onTweeningEnd;
        [SerializeField] private Image coveringImage;
        [SerializeField] private CameraGlitchController glitchController;
        [SerializeField] private float tweenDuration;

        private void OnEnable()
        {
            PlayerManager.Instance.StopInput();
            Sequence sequence = DOTween.Sequence();
            sequence.Append(coveringImage.DOFade(1, tweenDuration));
            sequence.Join(DOTween.To(() => glitchController.DigitalGlitchStrength,
                x => glitchController.DigitalGlitchStrength = x,
                1, tweenDuration));
            sequence.onKill += onTweeningEnd.Invoke;
            sequence.Play();
        }
    }
}