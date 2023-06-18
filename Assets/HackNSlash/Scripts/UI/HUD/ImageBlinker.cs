using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace HackNSlash.UI.DigitalWorld_HUD.Popups.Scripts
{
    public class ImageBlinker : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string blinkAnimProperty;
        [SerializeField] public UnityEvent onBlinkEnd;
        [SerializeField] public UnityEvent onBlinkStart;

        public void Blink()
        {
            onBlinkStart?.Invoke();
            animator.SetTrigger(blinkAnimProperty);
        }

        public IEnumerator SetOffCoroutine()
        {
            onBlinkEnd?.Invoke();
            yield return null;
            gameObject.SetActive(false);
        }
        public void SetOff() => StartCoroutine(SetOffCoroutine());
    }
}