using HackNSlash.Scripts.Player;
using UnityEngine;
using UnityEngine.Events;

public class PlotPanelInteractable : MonoBehaviour, IInteractable
{
    public PlayerInteraction PlayerInteraction { get; set; }
    public UnityEvent OnReact;
    public void React()
    {
        OnReact?.Invoke();
    }
}
