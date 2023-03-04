using HackNSlash.Scripts.GamePlayFlowManagement;
using UnityEngine;
using Utilities;

namespace HackNSlash.Scripts.GameManagement
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private SceneRefSO sceneRefs; 
        
        public bool isBooting = true;
        public bool isPaused = false;

        public SceneRefSO SceneManager => sceneRefs;
        
        public void LoadMainMenu()
        {
            isBooting = true;
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
    }
}