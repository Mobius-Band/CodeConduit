using System;
using HackNSlash.Scripts.GameManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject[] gamePlayElements;
    [SerializeField] private GameObject[] mainMenuElements;
    [SerializeField] private GameObject controlScreen;
    [Header("Main Menu Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button quitButton;
    [Space] 
    [SerializeField] private Button[] returnButtons;

    private void Start()
    {
        if (GameManager.Instance.isBooting)
        {
            ShowMainMenu();
            SetMainMenuButtons();  
            Array.ForEach(returnButtons, button => button.onClick.AddListener(ShowMainMenu));
        }
        else
        {
            HideAllMenus();
            StartGameplay();
        }
        
    }

    private void ShowMainMenu()
    {
        controlScreen.SetActive(false);
        Array.ForEach(gamePlayElements, ctx => ctx.SetActive(false));
        Array.ForEach(mainMenuElements, ctx => ctx.SetActive(true));
        GameManager.Instance.SetMousePointerForGameplay(false);
        GameManager.Instance.PauseGame();
    }

    public void StartGameplay()
    {
        Array.ForEach(gamePlayElements, ctx => ctx.SetActive(true));
        Array.ForEach(mainMenuElements, ctx => ctx.SetActive(false));
        controlScreen.SetActive(false);
        GameManager.Instance.isBooting = false;
        GameManager.Instance.SetMousePointerForGameplay(true);
        GameManager.Instance.ResumeGame();
    }
    
    public void ShowControls()
    {
        controlScreen.SetActive(true);
        Array.ForEach(mainMenuElements, ctx => ctx.SetActive(false));
    }
    
    public void HideControls()
    {
        controlScreen.SetActive(false);
        Array.ForEach(mainMenuElements, ctx => ctx.SetActive(true));
    }
    
    public void HideAllMenus()
    {
        Array.ForEach(mainMenuElements, ctx => ctx.SetActive(false));
        controlScreen.SetActive(false);
    }

    private void SetMainMenuButtons()
    {
        playButton.onClick.AddListener(StartGameplay);
        controlsButton.onClick.AddListener(ShowControls);
        quitButton.onClick.AddListener(GameManager.Instance.Quit);
    }
    
    
}
