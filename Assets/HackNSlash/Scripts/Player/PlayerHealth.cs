using HackNSlash.Scripts.GameManagement;
using Util;

namespace HackNSlash.Scripts.Player
{
    public class PlayerHealth : Health
    {
        new void Start()
        {
            base.Start();
            OnHealthChanged += PlayerStatsManager.Instance.SetHealthPercentage;
        }
        
        protected override void Die()
        {
            GameManager.Instance.SceneManager.LoadGameOverScene();
        }
    }
}