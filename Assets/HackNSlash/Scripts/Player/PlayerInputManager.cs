
using HackNSlash.Scripts.Util;

namespace Player
{
    public class PlayerInputManager : Singleton<PlayerInputManager>
    {
        public PlayerInputActions InputActions { get; private set; }
        private PlayerAttack _playerAttack;

        private new void Awake()
        {
            base.Awake();
            InputActions = new PlayerInputActions();
        }
        
        void OnEnable()
        {
            InputActions.Enable();
        }
        
        void OnDisable()
        {
            InputActions.Disable();
        }
        
        
    }
}