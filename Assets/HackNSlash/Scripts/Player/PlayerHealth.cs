using HackNSlash.Scripts.GameManagement;
using UnityEngine;
using Util;

namespace HackNSlash.Scripts.Player
{
    public class PlayerHealth : Health
    {
        private bool isImmortal;
        
        new void Start()
        {
            base.Start();
            OnHealthChanged += PlayerStatsManager.Instance.SetHealthPercentage;
        }
        
        protected override void Die()
        {
            if (isImmortal)
            {
                return;
            }
            GameManager.Instance.SceneManager.LoadGameOverScene();
        }

        public void ToggleImmortalMode()
        {
            isImmortal = !isImmortal;
            Debug.LogWarning("You are now immortal: " + isImmortal);
        }
    }
}