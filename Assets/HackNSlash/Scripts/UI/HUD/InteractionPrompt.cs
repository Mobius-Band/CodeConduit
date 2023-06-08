using HackNSlash.Scripts.Player;
using TMPro;
using UnityEngine;

namespace HackNSlash.Scripts.UI.HUD
{
    public class InteractionPrompt : MonoBehaviour
    {
        [TextArea][SerializeField] private string promptLabel;
        [SerializeField] private TMP_Text promptTextComponent;
        [SerializeField] private PlayerInteraction playerInteraction;

        private void LateUpdate()
        {
            SetText();
        }

        private bool TryToggleText()
        {
            promptTextComponent.enabled = playerInteraction.CanInteract;
            return promptTextComponent.enabled;
        }

        private void SetText()
        {
            if (TryToggleText())
            {
                promptTextComponent.text = promptLabel;
            }
        }
    }
}