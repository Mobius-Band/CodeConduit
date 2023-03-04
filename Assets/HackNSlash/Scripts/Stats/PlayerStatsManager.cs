using UnityEngine;

namespace HackNSlash.Scripts.GameManagement
{
    public class PlayerStatsManager : Singleton<PlayerStatsManager>
    {
        private string healthAccessor = "Health";
        
        public void SetHealthPercentage(int currentHealth, int maxHealth)
        {
            float healthPercentage = (float)currentHealth/maxHealth*100;
            PlayerPrefs.SetFloat(healthAccessor, healthPercentage);
        }
        
        public float GetHealthPercentage()
        {
            return PlayerPrefs.GetFloat(healthAccessor);
        }
    }
}