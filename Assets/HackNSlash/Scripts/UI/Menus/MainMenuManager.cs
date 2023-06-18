using System;
using DG.Tweening;
using HackNSlash.Scripts.Audio;
using HackNSlash.Scripts.GameManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject[] mainMenuElements;
    [SerializeField] private GameObject controlScreen; 
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Image overlayPanel;
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
        }
        
    }

    private void ShowMainMenu()
    {
        controlScreen.SetActive(false);
        Array.ForEach(mainMenuElements, ctx => ctx.SetActive(true));
        GameManager.Instance.SetMousePointerForGameplay(true);
        GameManager.Instance.PauseGame();
        EventSystem.current.SetSelectedGameObject(playButton.gameObject);
        audioManager.Play("openMainMenu", true);
    }

    private void StartGameplay()
    {
        Sequence playSequence = DOTween.Sequence();
        playSequence.SetUpdate(UpdateType.Late, true);
        playSequence.Append(overlayPanel.DOFade(1f, 3.5f));
        playSequence.onPlay += () => audioManager.Play("start");
        playSequence.onComplete += () => GameManager.Instance.SceneManager.LoadFirstGameplayScene();
        playSequence.Play();
    }
    
    public void ShowControls()
    {
        controlScreen.SetActive(true);
        Array.ForEach(mainMenuElements, ctx => ctx.SetActive(false));
        //TODO: Change for a better way to set the first selected button
        EventSystem.current.SetSelectedGameObject(returnButtons[0].gameObject);
        audioManager.Play("openSubMenu", true);
    }
    
    public void HideControls()
    {
        controlScreen.SetActive(false);
        Array.ForEach(mainMenuElements, ctx => ctx.SetActive(true));
        ShowMainMenu();
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
