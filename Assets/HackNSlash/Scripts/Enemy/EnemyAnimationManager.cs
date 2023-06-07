using System;
using UnityEngine;
using UnityEngine.VFX;

namespace HackNSlash.Scripts.Enemy
{
    public class EnemyAnimationManager : MonoBehaviour
    {
        [SerializeField] private VisualEffect dissolveVFX;
        public Action OnMouthAttack;
        public Action OnFinalTransformationEnd;

        private void MouthAttack()
        {
            OnMouthAttack?.Invoke();
        }

        private void FinalTransformationEnd()
        {
            OnFinalTransformationEnd?.Invoke();
        }

        private void PlayFinalDissolveVFX()
        {
            dissolveVFX.Play();
        }
    }
}