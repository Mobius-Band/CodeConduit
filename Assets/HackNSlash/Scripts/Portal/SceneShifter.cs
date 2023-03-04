using Eflatun.SceneReference;
using HackNSlash.Scripts.GameManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using PlayerInputManager = Player.PlayerInputManager;

namespace HackNSlash.Scripts.Portal
{
    public class SceneShifter : MonoBehaviour
    {
        [SerializeField] private SceneReference targetScene;
        [SerializeField] private bool shouldStartActive;
        [SerializeField] private UnityEvent OnActivation;
        [SerializeField] private UnityEvent OnDeactivation;

        private void Start()
        {
            DisableInteraction();
            SetActive(shouldStartActive);
        }

        public void SetActive(bool value)
        {
            if (value)
            {
                OnActivation?.Invoke();
            }
            else
            {
                OnDeactivation?.Invoke();
            }
        }

        public void EnableInteraction()
        {
            PlayerInputManager.Instance.InputActions.PuzzlePlayer.Interact.performed +=
                ShiftScene;
        }
        
        public void DisableInteraction()
        {
            PlayerInputManager.Instance.InputActions.PuzzlePlayer.Interact.performed -=
                ShiftScene;
        }

        private void ShiftScene(InputAction.CallbackContext ctx)
        {
            targetScene.SafeLoad();
        }
    }
}