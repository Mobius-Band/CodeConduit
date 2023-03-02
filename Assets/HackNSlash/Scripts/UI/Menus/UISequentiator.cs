using System;
using System.Collections;
using System.Collections.Generic;
using HackNSlash.Scripts.GameManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace HackNSlash.Scripts.TitleScreen
{
    public class UISequentiator : MonoBehaviour
    {
        [SerializeField] private GameObject[] sequentialScreens;
        [SerializeField] private float inputDelay = 0.2f;
        private int sequenceIndex = 0;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(inputDelay );
            InputSystem.onAnyButtonPress.CallOnce(ctx
                => StartCoroutine(SequentiateScreens(GameManager.Instance.LoadMainMenu)));
        }

        private IEnumerator SequentiateScreens(Action sequenceEndAction)
        {
            if (sequenceIndex >= sequentialScreens.Length - 1)
            {
                sequenceEndAction.Invoke();
                yield break;
            }
            sequentialScreens[sequenceIndex].SetActive(false);
            sequenceIndex++;
            sequentialScreens[sequenceIndex].SetActive(true);
            yield return new WaitForSeconds(inputDelay );
            InputSystem.onAnyButtonPress.CallOnce(ctx
                => StartCoroutine(SequentiateScreens(sequenceEndAction)));
        }

        private void SequentiateToMainMenu()
        {
            StartCoroutine(SequentiateScreens(GameManager.Instance.SceneManager.LoadMainMenu));
        }

        private IEnumerator DelayedSequentiationInput(Action sequenceEndAction)
        {
            yield return new WaitForSeconds(inputDelay);
            InputSystem.onAnyButtonPress.CallOnce(ctx
                => StartCoroutine(SequentiateScreens(sequenceEndAction)));
        }
    }
}