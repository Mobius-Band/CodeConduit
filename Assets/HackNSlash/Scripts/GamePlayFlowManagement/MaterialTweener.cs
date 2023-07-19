using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace HackNSlash.Scripts.GamePlayFlowManagement
{
    public class MaterialTweener : MonoBehaviour
    {
        [Tooltip("Leave it empty if the component's GameObject is the target one")]
        [SerializeField] private GameObject targetGO;
        [Space]
        [SerializeField] private int materialIndex = 0;
        [SerializeField] private float tweenDuration;
        [ColorUsage(false, true)][SerializeField] private Color targetColor;
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
            if (targetGO == null)
            {
                targetGO = gameObject;
            }
            material = targetGO.GetComponent<Renderer>().materials[materialIndex];
            originalColor = material.GetColor(propertyName);
        }

        private void OnValidate()
        {
            if (targetGO == null)
            {
                targetGO = gameObject;
            }
        }

        public void TweenTowardsTargetColor()
        {
            Debug.Log(name);
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