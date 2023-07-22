using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace HackNSlash.Scripts.GamePlayFlowManagement
{
    public abstract class AestheticTweener : MonoBehaviour
    {
        [Tooltip("Leave it empty if the component's GameObject is the target one")]
        [SerializeField] protected GameObject targetGO;
        [Space]
        [SerializeField] protected float tweenDuration;
        [ColorUsage(false, true)][SerializeField] protected Color targetColor;
        [SerializeField] protected string propertyName;
        [SerializeField] protected bool ignoreExternalSettings;
        
        protected Color originalColor;
        protected Tween colorTween;
        
        public UnityEvent OnTweeningToTarget;
        public UnityEvent OnTweenedToTarget;
        public UnityEvent OnTweeningToOriginal;
        public UnityEvent OnTweenedToOriginal;

        protected void Awake()
        {
            NullCoalesce();
            InitializeAestheticsProperties();
        }

        private void NullCoalesce()
        {
            if (targetGO == null)
            {
                targetGO = gameObject;
            }
        }
        
        protected abstract void InitializeAestheticsProperties();

        protected abstract void InitializeTargetTween();

        protected abstract void InitializeOriginalTween();
        
        public void TweenTowardsTargetColor()
        {
            colorTween.Kill();
            InitializeTargetTween();
            colorTween.onPlay += OnTweeningToTarget.Invoke;
            colorTween.onKill += OnTweenedToTarget.Invoke;
        }
        
        public void TweenTowardsOriginalColor()
        {
            colorTween.Kill();
            InitializeOriginalTween();
            colorTween.onPlay += OnTweeningToOriginal.Invoke;
            colorTween.onKill += OnTweenedToOriginal.Invoke;
        }
        
        public void TrySetColorAndDuration(Color color, float duration)
        {
            if (ignoreExternalSettings)
            {
                return;
            }
            this.targetColor = color;
            this.tweenDuration = duration;
        }
    }
}