using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace HackNSlash.Scripts.Enemy
{
    public class EnemyAnimationManager : MonoBehaviour
    {
        public UnityEvent OnMouthAttackBegin;
        public Action OnMouthAttack;
        public Action OnFinalTransformationEnd;
        public UnityEvent OnDeathBegin;
        public UnityEvent OnDeathEnd;

        private void MouthAttackBegin()
        {
            OnMouthAttackBegin?.Invoke();
        }
        private void MouthAttack()
        {
            OnMouthAttack?.Invoke();
        }

        private void FinalTransformationEnd()
        {
            OnFinalTransformationEnd?.Invoke();
        }

        private void DeathBegin()
        {
            OnDeathBegin?.Invoke();
        }

        private void DeathEnd()
        {
            OnDeathEnd?.Invoke();
        }

        private void OnDisable()
        {
            OnMouthAttackBegin.RemoveAllListeners();
            OnDeathBegin.RemoveAllListeners();
            OnDeathEnd.RemoveAllListeners();
        }
    }
}