using DG.Tweening;
using UnityEngine;

namespace HackNSlash.Scripts.GamePlayFlowManagement
{
    public class MaterialTweener : AestheticTweener
    {
        [SerializeField] private int materialIndex = 0;

        private Material material;

        protected override void InitializeAestheticsProperties()
        {
            material = targetGO.GetComponent<Renderer>().materials[materialIndex];
            originalColor = material.GetColor(propertyName);
        }

        protected override void InitializeTargetTween()
        {
            colorTween = material.DOColor(targetColor, propertyName, tweenDuration);
        }

        protected override void InitializeOriginalTween()
        {
            colorTween = material.DOColor(originalColor, propertyName, tweenDuration);
        }
        
    }
}