using System;
using Combat;
using HackNSlash.Scripts.Player;
using HackNSlash.Scripts.UI;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerInputManager))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerAnimationManager _playerAnimationManager;
        [SerializeField] private bool _isPuzzlePlayer;
        private PlayerInputManager _input;
        private ComboManager _comboManager;
        private PlayerMovement _movement;
        private PlayerPickupSphere _playerPickupSphere;

        [Header("External References")] 
        [SerializeField] private PauseMenuManager _pauseMenu;
        
        void Awake()
        {
            _input = GetComponent<PlayerInputManager>();
            _comboManager = GetComponent<ComboManager>();
            _movement = GetComponent<PlayerMovement>();
            _playerPickupSphere = GetComponent<PlayerPickupSphere>();
        }
        
        void Start()
        {
            //External
            _input.InputActions.Player.Pause.performed += _ => _pauseMenu.TogglePauseMenu();
            
            if (_isPuzzlePlayer)
            {
                // create interaction function
                _input.InputActions.PuzzlePlayer.Interact.performed += _ => _playerPickupSphere.SphereInteract();
                return;
            }
            
            _input.InputActions.Player.Attack.performed += _ => _comboManager.HandleAttackInput();
            _input.InputActions.Player.Dash.performed += _ => _movement.Dash();

            if (_playerAnimationManager != null)
            {
                _playerAnimationManager.OnAnimationEndCombo += _comboManager.EndCombo;
                _playerAnimationManager.OnAnimationHit += _comboManager.ToggleHitbox;
                _playerAnimationManager.OnAnimationSuspendRotation += _movement.SuspendRotation;
                _playerAnimationManager.OnAnimationReturningToIdle += _comboManager.SetReturningToIdle;
                _playerAnimationManager.OnAnimationEndCombo += _comboManager.EndCombo;
                _playerAnimationManager.OnAnimationHit += _comboManager.ToggleHitbox;
                _playerAnimationManager.OnAnimationSuspendRotation += _movement.SuspendRotation;
                _playerAnimationManager.OnAnimationReturningToIdle += _comboManager.SetReturningToIdle;
                _playerAnimationManager.OnAnimationEndDash += _movement.EndDash;
                _playerAnimationManager.OnAnimationSetNextAttack += _comboManager.SetNextAttack;
                _playerAnimationManager.OnAnimationAttackStep += () => _movement.AttackStep(_comboManager.CurrentAttack);
                _playerAnimationManager.OnAnimationSuspendMovement += _movement.SuspendMovement;
                _playerAnimationManager.OnAnimationRegainMovement += _movement.RegainMovement;
            }
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
            if (_isPuzzlePlayer)
            {
                _movement.MoveInput = _input.InputActions.PuzzlePlayer.Move.ReadValue<Vector2>();
                return;
            }
            
            _movement.MoveInput = _input.InputActions.Player.Move.ReadValue<Vector2>();
        }
    }
}