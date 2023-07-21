using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace HackNSlash.Scripts.GamePlayFlowManagement
{
    [RequireComponent(typeof(VisualEffect))]
    [Serializable]
    public class VFXTweener : AestheticTweener
    {
        private VisualEffect _vfx;

        protected override void InitializeAestheticsProperties()
        {
            _vfx = targetGO.GetComponent<VisualEffect>();
            originalColor = _vfx.GetVector4(propertyName);
        }

        protected override void InitializeTargetTween()
        {
             colorTween = DOTween.To(() => _vfx.GetVector4(propertyName), x => _vfx.SetVector4(propertyName, x),
                (Vector4)targetColor, tweenDuration);
        }

        protected override void InitializeOriginalTween()
        {
            colorTween = DOTween.To(() => _vfx.GetVector4(propertyName), x => _vfx.SetVector4(propertyName, x),
                (Vector4)originalColor, tweenDuration);  
        }
    }
}