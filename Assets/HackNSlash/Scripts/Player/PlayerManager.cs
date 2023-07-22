using System;
using Combat;
using HackNSlash.Scripts.Puzzle;
using HackNSlash.Scripts.UI;
using Player;
using UnityEngine;

namespace HackNSlash.Scripts.Player
{
    [RequireComponent(typeof(PlayerInputManager))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerAnimationManager playerAnimationManager;
        [SerializeField] private SphereElevator sphereElevator;
        [SerializeField] private bool isPuzzlePlayer;
        [SerializeField] private Renderer[] playerRenderers;
        [SerializeField] private Collider[] playerColliders;
        
        private PlayerInputManager _input;
        private ComboManager _comboManager;
        private PlayerMovement _movement;
        private PlayerPickupSphere _playerPickupSphere;
        private PlayerHealth _playerHealth;
        private PlayerInteraction _playerInteraction;

        [Header("External References")] 
        [SerializeField] private PauseMenuManager pauseMenu;
        [SerializeField] private GameObject deathScreen;
        
        void Awake()
        {
            _input = GetComponent<PlayerInputManager>();
            _comboManager = GetComponent<ComboManager>();
            _movement = GetComponent<PlayerMovement>();
            _playerPickupSphere = GetComponent<PlayerPickupSphere>();
            _playerHealth = GetComponent<PlayerHealth>();
            _playerInteraction = GetComponent<PlayerInteraction>();
        }
        
        void Start()
        {
            InitializeGeneralWorkflow();
            InitializePropInteraction();
            if (!isPuzzlePlayer)
            {
                InitializeCombatInput();
            }
            InitializeAnimationEvents();
            InitializeCheatCodes();
        }

        private void InitializeGeneralWorkflow()
        {
            _input.InputActions.Player.Pause.performed += _ => pauseMenu.TogglePauseMenu();
            _playerHealth.OnPlayerDeath.AddListener(KillPlayer);
        }
        
        private void InitializePropInteraction()
        {
            if (isPuzzlePlayer)
            {
                _input.InputActions.PuzzlePlayer.Interact.performed += _ => _playerInteraction.Interact();
                _input.InputActions.PuzzlePlayer.Interact.performed += _ => _playerPickupSphere.SphereInteract();
                if (sphereElevator && !_playerPickupSphere.isHoldingSphere)
                {
                    _input.InputActions.PuzzlePlayer.Interact.performed += _ => sphereElevator.ElevatorActivate();
                }
            }
        }
        
        private void InitializeAnimationEvents()
        {
            if (playerAnimationManager == null)
            {
                Debug.LogError("Player animation manager hasn't been found!");
                return;
            }
            playerAnimationManager.OnAnimationMovementStep += _movement.PlayStepSound;
            if (isPuzzlePlayer)
            {
                //Restricted puzzle animation events
            }
            else
            {
                playerAnimationManager.OnAnimationEndCombo += _comboManager.EndCombo;
                playerAnimationManager.OnAnimationHit += _comboManager.ToggleHitbox;
                playerAnimationManager.OnAnimationSuspendRotation += _movement.SuspendRotation;
                playerAnimationManager.OnAnimationReturningToIdle += _comboManager.SetReturningToIdle;
                playerAnimationManager.OnAnimationEndCombo += _comboManager.EndCombo;
                playerAnimationManager.OnAnimationHit += _comboManager.ToggleHitbox;
                playerAnimationManager.OnAnimationEndDash += _movement.EndDash;
                playerAnimationManager.OnAnimationSetNextAttack += _comboManager.SetNextAttack;
                playerAnimationManager.OnAnimationAttackStep += () => _movement.AttackStep(_comboManager.currentAttack);
                playerAnimationManager.OnAnimationSuspendMovement += _movement.SuspendMovement;
                playerAnimationManager.OnAnimationRegainMovement += _movement.RegainMovement;
            }
        }

        private void InitializeCombatInput()
        {
            _input.InputActions.Player.AttackLight.performed += _ => _comboManager.HandleAttackInput(true);
            _input.InputActions.Player.AttackHeavy.performed += _ => _comboManager.HandleAttackInput(false);
            _input.InputActions.Player.Dash.performed += _ => _movement.Dash();
        }

        private void InitializeCheatCodes()
        {
            if (isPuzzlePlayer)
            {
                
            }
            else
            {
                _input.InputActions.Player.CHEATCODEInfiniteHealth.performed += _ => _playerHealth.ToggleImmortalMode();
            }
        }

        private void OnDisable()
        {
            playerAnimationManager.OnAnimationEndCombo -= _comboManager.EndCombo;
            playerAnimationManager.OnAnimationHit -= _comboManager.ToggleHitbox;
            playerAnimationManager.OnAnimationSuspendRotation -= _movement.SuspendRotation;
            playerAnimationManager.OnAnimationReturningToIdle -= _comboManager.SetReturningToIdle;
            playerAnimationManager.OnAnimationEndDash -= _movement.EndDash;
            playerAnimationManager.OnAnimationSetNextAttack -= _comboManager.SetNextAttack;
            playerAnimationManager.OnAnimationAttackStep -= () => _movement.AttackStep(_comboManager.currentAttack);
        }

        void Update()
        {
            if (isPuzzlePlayer)
            {
                _movement.MoveInput = _input.InputActions.PuzzlePlayer.Move.ReadValue<Vector2>();
                return;
            }
            
            _movement.MoveInput = _input.InputActions.Player.Move.ReadValue<Vector2>();
        }

        private void KillPlayer()
        {
            Array.ForEach(playerRenderers, r => r.enabled = false);
            Array.ForEach(playerColliders, c => c.enabled = false);
            StopInput();
            deathScreen.SetActive(true);
        }

        public void StopInput() => _input.enabled = false;
        public void ResumeInput() => _input.enabled = true;
    }
}