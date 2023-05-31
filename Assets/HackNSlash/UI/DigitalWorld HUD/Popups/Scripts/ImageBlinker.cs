using System;
using UnityEngine;

namespace HackNSlash.UI.DigitalWorld_HUD.Popups.Scripts
{
    public class ImageBlinker : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string blinkAnimProperty;

        public void Blink()
        {
            animator.SetTrigger(blinkAnimProperty);
        }

        private void OnEnable()
        {
            Blink();
        }

        public void SetOff()
        {
            gameObject.SetActive(false);
        }
    }
}