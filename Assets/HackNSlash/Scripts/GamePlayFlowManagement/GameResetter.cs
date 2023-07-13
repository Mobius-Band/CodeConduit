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
        public void ResetToPreviousSceneOrToMainMenu()
        {
            Debug.Log("Resetting this shit@");
            int previousSceneIndex = GameManager.Instance.SceneManager.previousSceneIndex;
            if (previousSceneIndex < 0)
            {
                GameManager.Instance.LoadMainMenu();
                return;
            }
            SceneManager.LoadScene(previousSceneIndex);
        }
    }
}