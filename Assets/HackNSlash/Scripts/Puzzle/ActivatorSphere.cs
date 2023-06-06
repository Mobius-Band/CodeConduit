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

        private readonly String _sphereElevatorSceneDown = "Part4-2-1-1";
        private readonly String _sphereElevatorSceneUp = "Part4-2-2";

        private void Awake()
        {
            if (SceneManager.GetActiveScene().name == _sphereElevatorSceneDown)
            {
                gameObject.SetActive(IsDown);
            }
            else if (SceneManager.GetActiveScene().name == _sphereElevatorSceneUp)
            {
                gameObject.SetActive(!IsDown);
            }
        }
    }
}
