using HackNSlash.Scripts.GameManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HackNSlash.Scripts.UI
{
    public class PauseMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenuCanvas;
        [Header("Buttons")]
        [SerializeField] private Button continueButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button controlsButton;
        [SerializeField] private Button toMenuButton;
        [Header("PauseSubMenu")]
        [SerializeField] private GameObject controlsSubmenu;
        [SerializeField] private Button controlsReturnButton;
        
        private void Start()
        {
            Resume();
            continueButton.onClick.AddListener(Resume);
            restartButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
            controlsButton.onClick.AddListener(ShowControls);
            controlsReturnButton.onClick.AddListener(HideControls);
            toMenuButton.onClick.AddListener(GameManager.Instance.LoadMainMenu);
        }

        private void Pause()
        {
            ShowPauseMenu();
            GameManager.Instance.PauseGame();
        }
        
        private void Resume()
        {
            HidePauseMenu();
            controlsSubmenu.SetActive(false);
            GameManager.Instance.ResumeGame();
        }

        public void TogglePauseMenu()
        {
            if (GameManager.Instance.isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        
        public void ShowControls()
        {
            controlsSubmenu.SetActive(true);
            HidePauseMenu();
            EventSystem.current.SetSelectedGameObject(controlsReturnButton.gameObject);
        }
    
        public void HideControls()
        {
            controlsSubmenu.SetActive(false);
            ShowPauseMenu();
        }

        public void ShowPauseMenu()
        {
            pauseMenuCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
        }
        
        public void HidePauseMenu()
        {
            pauseMenuCanvas.SetActive(false);
        }
    }
}