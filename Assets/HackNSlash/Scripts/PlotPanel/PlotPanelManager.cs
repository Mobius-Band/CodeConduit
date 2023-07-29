using System;
using System.Collections;
using HackNSlash.Scripts.GameManagement;
using HackNSlash.Scripts.UI.Menus;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HackNSlash.Scripts.PlotPanel
{
    public class PlotPanelManager : MonoBehaviour
    {
        [SerializeField] private PlotPanelInteractable interactable;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Button exitButton;
        [SerializeField] private ScrollInput scrollInput;

        private Coroutine togglerRoutine;

        private void OnEnable()
        {
            DeactivatePlotPanel();
            interactable.OnReact.AddListener(ActivatePlotPanel);
        }

        private IEnumerator TogglePlotPanel(bool value)
        {
            canvas.enabled = value;
            yield return new WaitForSecondsRealtime(0.1f);
            scrollInput.enabled = value;
            GameManager.Instance.CanUsePauseMenu = !value;
            if (value)
            {
                EventSystem.current.SetSelectedGameObject(exitButton.gameObject);
                GameManager.Instance.PauseGame();
                yield return new WaitForSecondsRealtime(0.1f);
                exitButton.onClick.AddListener(DeactivatePlotPanel);

            }
            else
            {
                GameManager.Instance.ResumeGame();
                exitButton.onClick.RemoveAllListeners();
            }
        }

        private void TogglePlotPanelCoroutine(bool value)
        {
            if (togglerRoutine != null)
            {
                StopCoroutine(togglerRoutine);
            }
            togglerRoutine = StartCoroutine(TogglePlotPanel(value));
        }
        private void ActivatePlotPanel() => TogglePlotPanelCoroutine(true);
        private void DeactivatePlotPanel() => TogglePlotPanelCoroutine(false);
    }
}