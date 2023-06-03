using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackNSlash.Scripts.Puzzle
{
    public class ActivatorSphere : MonoBehaviour
    {
        public int index;
        public bool isBeingHeld;
        public bool isDown;
        public bool isOnElevator;
        private readonly String _sphereElevatorScene1 = "Part4-2-1-1";
        private readonly String _sphereElevatorScene2 = "Part4-2-2";

        private void Awake()
        {
            if (SceneManager.GetActiveScene().name == _sphereElevatorScene1)
            {
                if (isDown) return;
                gameObject.SetActive(false);
            }
            else if (SceneManager.GetActiveScene().name == _sphereElevatorScene2)
            {
                if (!isDown) return;
                gameObject.SetActive(false);
            }
        }
    }
}
