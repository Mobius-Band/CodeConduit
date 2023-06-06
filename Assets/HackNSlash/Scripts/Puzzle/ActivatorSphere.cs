using System;
using HackNSlash.Scripts.GameManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace HackNSlash.Scripts.Puzzle
{
    public class ActivatorSphere : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [HideInInspector] public bool isBeingHeld;
        public int sphereIndex;
        public float dropHeight;
        public bool IsDown => gameManager.SphereElevatorState.sphereIsDown[sphereIndex];


        private void Awake()
        {
            if (SceneManager.GetActiveScene().name == gameManager._sphereElevatorSceneDown)
            {
                gameObject.SetActive(IsDown);
            }
            else if (SceneManager.GetActiveScene().name == gameManager._sphereElevatorSceneUp)
            {
                gameObject.SetActive(!IsDown);
            }
        }
    }
}
