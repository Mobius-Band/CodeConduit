using HackNSlash.ScriptableObjects;
using HackNSlash.Scripts.GamePlayFlowManagement;
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
        [SerializeField] private string laserWallScene1, laserWallScene2;
        
        [Header("GAMEFLOW SETUP")]
        public bool isBooting = true;
        public bool isPaused = false;
        
        [Header("SCENE TRANSITION EVENTS")] 
        [SerializeField] private UnityEvent OnMainMenuLoaded;

        [Header("ELEVATOR STATE SETUP")] [SerializeField]
        private ElevatorState _elevatorState;
        
        public SceneRefSO SceneManager => sceneRefs;
        public AccessData AccessData => _accessData;
        public ElevatorState ElevatorState => _elevatorState;
        
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
            SetMousePointerForGameplay(false);
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
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == laserWallScene1)
            {
                AccessData.canAccessPart2 = true;
            }
            
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == laserWallScene2)
            {
                AccessData.canAccessPart3 = true;
            }
        }
    }
}