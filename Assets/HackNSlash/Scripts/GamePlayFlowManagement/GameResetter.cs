using System;
using HackNSlash.Scripts.GameManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

namespace HackNSlash.Scripts.GamePlayFlowManagement
{
    public class GameResetter : MonoBehaviour
    {

        private void OnEnable()
        {
            int previousSceneIndex = GameManager.Instance.SceneManager.previousSceneIndex;
            if (previousSceneIndex < 0)
            {
                InputSystem.onAnyButtonPress.CallOnce(_
                    => GameManager.Instance.LoadMainMenu());
                return;
            }
            InputSystem.onAnyButtonPress.CallOnce(_
                => SceneManager.LoadScene(previousSceneIndex));
        }
    }
}