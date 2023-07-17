using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace HackNSlash.Scripts.GamePlayFlowManagement
{
    public class MaterialTweener : MonoBehaviour
    {
        [SerializeField] private int materialIndex = 0;
        [SerializeField] private float tweenDuration;
        [SerializeField] private Color targetColor;
        [SerializeField] private string propertyName;

        private Material material;
        private Color originalColor;
        private Tween colorTween;

        public UnityEvent OnTweeningToTarget;
        public UnityEvent OnTweenedToTarget;
        public UnityEvent OnTweeningToOriginal;
        public UnityEvent OnTweenedToOriginal;

        private void Awake()
        {
            material = GetComponent<Renderer>().materials[materialIndex];
            originalColor = material.GetColor(propertyName);
        }

        public void TweenTowardsTargetColor()
        {
            colorTween.Kill();
            colorTween = material.DOColor(targetColor, propertyName, tweenDuration);
            colorTween.onPlay += OnTweeningToTarget.Invoke;
            colorTween.onKill += OnTweenedToTarget.Invoke;
        }
        
        public void TweenTowardsOriginalColor()
        {
            colorTween.Kill();
            colorTween = material.DOColor(originalColor, propertyName, tweenDuration);
            colorTween.onPlay += OnTweeningToOriginal.Invoke;
            colorTween.onKill += OnTweenedToOriginal.Invoke;
        }
    }
}