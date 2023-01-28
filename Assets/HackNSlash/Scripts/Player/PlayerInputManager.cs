using UnityEngine;

namespace Player
{
    public class PlayerInputManager : Singleton<PlayerInputManager>
    {
        public PlayerInputActions InputActions { get; private set; }
        private PlayerAttack _playerAttack;
        
        new void Awake()
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