using System;
using Combat;
using HackNSlash.Scripts.UI;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerInputManager))]
    [RequireComponent(typeof(ComboManager))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerAnimationManager _playerAnimationManager;
        private PlayerInputManager _input;
        private ComboManager _comboManager;
        private PlayerMovement _movement;

        [Header("External References")] 
        [SerializeField] private PauseMenuManager _pauseMenu;
        
        void Awake()
        {
            _input = GetComponent<PlayerInputManager>();
            _comboManager = GetComponent<ComboManager>();
            _movement = GetComponent<PlayerMovement>();
        }
        
        void Start()
        {
            _input.InputActions.Player.Attack.performed += _ => _comboManager.HandleAttackInput();
            _input.InputActions.Player.Dash.performed += _ => _movement.Dash();

            _playerAnimationManager.OnAnimationEndCombo += _comboManager.EndCombo;
            _playerAnimationManager.OnAnimationHit += _comboManager.ToggleHitbox;
            _playerAnimationManager.OnAnimationSuspendRotation += _movement.SuspendRotation;
            _playerAnimationManager.OnAnimationReturningToIdle += _comboManager.SetReturningToIdle;
            _playerAnimationManager.OnAnimationEndDash += _movement.EndDash;
            _playerAnimationManager.OnAnimationSetNextAttack += _comboManager.SetNextAttack;
            _playerAnimationManager.OnAnimationAttackStep += () => _movement.AttackStep(_comboManager.CurrentAttack);
            
            //External
            _input.InputActions.Player.Pause.performed += _ => _pauseMenu.TogglePauseMenu();
        }

        private void OnDisable()
        {
            _playerAnimationManager.OnAnimationEndCombo -= _comboManager.EndCombo;
            _playerAnimationManager.OnAnimationHit -= _comboManager.ToggleHitbox;
            _playerAnimationManager.OnAnimationSuspendRotation -= _movement.SuspendRotation;
            _playerAnimationManager.OnAnimationReturningToIdle -= _comboManager.SetReturningToIdle;
            _playerAnimationManager.OnAnimationEndDash -= _movement.EndDash;
            _playerAnimationManager.OnAnimationSetNextAttack -= _comboManager.SetNextAttack;
            _playerAnimationManager.OnAnimationAttackStep -= () => _movement.AttackStep(_comboManager.CurrentAttack);
        }

        void Update()
        {
            _movement.MoveInput = _input.InputActions.Player.Move.ReadValue<Vector2>();
        }
    }
}