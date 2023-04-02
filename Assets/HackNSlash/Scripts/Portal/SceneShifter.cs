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
        [SerializeField] private int accessorSceneIndex = -1;
        [SerializeField] private UnityEvent OnActivation;
        [SerializeField] private UnityEvent OnDeactivation;

        private void Start()
        {
            DisableInteraction();
            SetActive(shouldStartActive && CanBeAccessed());
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

        private bool CanBeAccessed()
        {
            switch ((uint)accessorSceneIndex)
            {
                case 2:
                    if (GameManager.Instance.AccessData.canAccessPart2)
                    {
                        return false;
                    }
                    break;
                case 3:
                    if (GameManager.Instance.AccessData.canAccessPart3)
                    {
                        return false;
                    }
                    break;
            }
            return true;
        }
    }
}