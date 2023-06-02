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
        private PlayerInputManager _input;
        private ComboManager _comboManager;
        private PlayerMovement _movement;
        private PlayerPickupSphere _playerPickupSphere;

        [Header("External References")] 
        [SerializeField] private PauseMenuManager pauseMenu;
        
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
            _input.InputActions.Player.Pause.performed += _ => pauseMenu.TogglePauseMenu();
            
            if (isPuzzlePlayer)
            {
                // create interaction function
                _input.InputActions.PuzzlePlayer.Interact.performed += _ => _playerPickupSphere.SphereInteract();
                if (sphereElevator.canPressButton) _input.InputActions.PuzzlePlayer.Interact.performed += _ => sphereElevator.ActivateButton();
                return;
            }
            
            _input.InputActions.Player.Attack.performed += _ => _comboManager.HandleAttackInput();
            _input.InputActions.Player.Dash.performed += _ => _movement.Dash();

            if (playerAnimationManager != null)
            {
                playerAnimationManager.OnAnimationEndCombo += _comboManager.EndCombo;
                playerAnimationManager.OnAnimationHit += _comboManager.ToggleHitbox;
                playerAnimationManager.OnAnimationSuspendRotation += _movement.SuspendRotation;
                playerAnimationManager.OnAnimationReturningToIdle += _comboManager.SetReturningToIdle;
                playerAnimationManager.OnAnimationEndCombo += _comboManager.EndCombo;
                playerAnimationManager.OnAnimationHit += _comboManager.ToggleHitbox;
                playerAnimationManager.OnAnimationSuspendRotation += _movement.SuspendRotation;
                playerAnimationManager.OnAnimationReturningToIdle += _comboManager.SetReturningToIdle;
                playerAnimationManager.OnAnimationEndDash += _movement.EndDash;
                playerAnimationManager.OnAnimationSetNextAttack += _comboManager.SetNextAttack;
                playerAnimationManager.OnAnimationAttackStep += () => _movement.AttackStep(_comboManager.CurrentAttack);
                playerAnimationManager.OnAnimationSuspendMovement += _movement.SuspendMovement;
                playerAnimationManager.OnAnimationRegainMovement += _movement.RegainMovement;
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
            playerAnimationManager.OnAnimationAttackStep -= () => _movement.AttackStep(_comboManager.CurrentAttack);
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
    }
}