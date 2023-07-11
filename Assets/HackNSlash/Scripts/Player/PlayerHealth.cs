using System.Collections;
using HackNSlash.Scripts.GameManagement;
using HackNSlash.Scripts.Util;
using UnityEngine;
using UnityEngine.Events;
using Util;

namespace HackNSlash.Scripts.Player
{
    public class PlayerHealth : Health
    {
        public UnityEvent OnPlayerDeath;
        private bool isImmortal;

        void Start()
        {
            OnHealthChanged += PlayerStatsManager.Instance.SetHealthPercentage;
        }
        
        protected override IEnumerator Die()
        {
            if (isImmortal)
            {
                yield break;
            }
            OnPlayerDeath?.Invoke();
        }

        public void ToggleImmortalMode()
        {
            isImmortal = !isImmortal;
            Debug.LogWarning("You are now immortal: " + isImmortal);
        }
    }
}