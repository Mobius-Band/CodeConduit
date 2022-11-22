﻿using System;
using HackNSlash.Scripts.GameManagement;
using UnityEngine;
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
            toMenuButton.onClick.AddListener(GameManager.Instance.ReloadGame);
        }

        private void Pause()
        {
            pauseMenuCanvas.SetActive(true);
            GameManager.Instance.PauseGame();
        }
        
        private void Resume()
        {
            pauseMenuCanvas.SetActive(false);
            GameManager.Instance.ResumeGame();
        }

        public void TogglePauseMenu()
        {
            if (pauseMenuCanvas.activeSelf)
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
            pauseMenuCanvas.SetActive(false);
        }
    
        public void HideControls()
        {
            controlsSubmenu.SetActive(false);
            pauseMenuCanvas.SetActive(true);
        }
    }
}