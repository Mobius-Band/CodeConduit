using System;
using UnityEngine;

namespace HackNSlash.Scripts.Enemy
{
    public class EnemyAnimationManager : MonoBehaviour
    {
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
    }
}