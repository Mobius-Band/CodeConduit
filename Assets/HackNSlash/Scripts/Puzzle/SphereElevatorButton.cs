using System;
using UnityEngine;

namespace HackNSlash.Scripts.Puzzle
{
    public class SphereElevatorButton : MonoBehaviour
    {
        [SerializeField] private SphereElevator sphereElevator;
        public bool canPressButton = false;

        public void ActivateButton()
        {
            if (canPressButton)
            {
                sphereElevator.ElevatorActivate();
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                canPressButton = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                canPressButton = false;
            }
        }
    }
}
