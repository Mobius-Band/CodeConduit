using System;
using HackNSlash.Scripts.GameManagement;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using PlayerInputManager = Player.PlayerInputManager;
using SceneReference = Eflatun.SceneReference.SceneReference;

namespace HackNSlash.Scripts.Portal
{
    public class SceneShifter : MonoBehaviour
    {
        [Header("BASE SETUP")]
        [SerializeField] private SceneReference targetScene;
        [SerializeField] private bool shouldStartActive;
        [SerializeField] private int accessorSceneIndex = -1;
        
        [Header("EVENTS")]
        [SerializeField] private UnityEvent OnActivation;
        [SerializeField] private UnityEvent OnDeactivation;

        [Header("GAME EVENTS")] 
        [SerializeField] private GameEvent sceneShiftStarted;

        private bool isReadyToShift;

        private void Start()
        {
            isReadyToShift = false;
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
                StartSceneShifting;
        }
        
        public void DisableInteraction()
        {
            PlayerInputManager.Instance.InputActions.PuzzlePlayer.Interact.performed -=
                StartSceneShifting;
        }

        private void StartSceneShifting(InputAction.CallbackContext ctx)
        {
            GameManager.Instance.SceneManager.DefinePreviousScene(SceneManager.GetActiveScene());
            isReadyToShift = true;
            sceneShiftStarted.Raise();
        }

        public void EndSceneShifting()
        {
            if (!isReadyToShift)
            {
                return;
            }
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
                case 4:
                    if (GameManager.Instance.AccessData.canAccessPart4)
                    {
                        return false;
                    }
                    break;
            }
            return true;
        }
    }
}