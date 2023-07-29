using System;
using Eflatun.SceneReference;
using HackNSlash.ScriptableObjects;
using HackNSlash.Scripts.GamePlayFlowManagement;
using HackNSlash.Scripts.Util;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace HackNSlash.Scripts.GameManagement
{
    public class GameManager : Singleton<GameManager>
    {
        
        [SerializeField] private SceneRefSO sceneRefs;
        
        [Header("AREA ACCESS SETUP")]
        [SerializeField] private AccessData _accessData;

        [Header("GAMEFLOW SETUP")]
        public bool isBooting = true;
        public bool isPaused = false;
        
        [Header("SCENE TRANSITION EVENTS")] 
        [SerializeField] private UnityEvent OnMainMenuLoaded;

        [Header("ELEVATOR STATE SETUP")] 
        [SerializeField] private ElevatorState _elevatorState;

        [SerializeField] private SphereElevatorState _sphereElevatorState;

        public SceneRefSO SceneManager => sceneRefs;
        public AccessData AccessData => _accessData;
        public ElevatorState ElevatorState => _elevatorState;

        public SphereElevatorState SphereElevatorState => _sphereElevatorState;

        [Header("ELEVATOR SCENES")]
        [SerializeField] public SceneReference sphereElevatorSceneDown;
        [SerializeField] public SceneReference sphereElevatorSceneUp;
        [SerializeField] public SceneReference playerElevatorSceneDown;
        [SerializeField] public SceneReference playerElevatorSceneUp;

        public bool CanUsePauseMenu = true;
        
        public void Start()
        {
            SetMousePointerForGameplay(true);
            isBooting = true;
            if (!Application.isEditor)
            {
                OnMainMenuLoaded?.Invoke();
            }
        }

        public void LoadMainMenu()
        {
            isBooting = true;
            OnMainMenuLoaded?.Invoke();
            sceneRefs.LoadMainMenu();
        }
        
        public void PauseGame()
        {
            Time.timeScale= 0;
            isPaused = true;
            SetMousePointerForGameplay(true);
        }
    
        public void ResumeGame()
        {
            Time.timeScale = 1;
            isPaused = false;
            SetMousePointerForGameplay(true);
        }

        public void Quit()
        {
            AppHelper.Quit();
        }

        public void SetMousePointerForGameplay(bool doIt)
        {
            Cursor.visible = !doIt;
            Cursor.lockState = !doIt ? CursorLockMode.None : CursorLockMode.Locked;
        }

        public void UnlockCurrentLaserWall()
        {
            var activeSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            if (activeSceneIndex == AccessData.DigitalPart2Scene.BuildIndex)
            {
                AccessData.canAccessPart2 = true;
            }
            
            if (activeSceneIndex == AccessData.DigitalPart3Scene.BuildIndex)
            {
                AccessData.canAccessPart3 = true;
            }
            
            if (activeSceneIndex == AccessData.DigitalPart4Scene.BuildIndex)
            {
                AccessData.canAccessPart4 = true;
            }
        }

        // public AsyncOperation SafeLoadAsync(SceneReference sceneReference)
        // {
        //     LoadMainMenu(
        // }
    }
}