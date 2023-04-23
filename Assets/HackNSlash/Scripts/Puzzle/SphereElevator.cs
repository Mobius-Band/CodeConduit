using System;
using HackNSlash.Scripts.GameManagement;
using HackNSlash.Scripts.Player;
using UnityEngine;

namespace HackNSlash.Scripts.Puzzle
{
    public class SphereElevator : MonoBehaviour
    {
        [SerializeField] private PlayerPickupSphere _playerPickupSphere;
        private bool _hasSphere;
        
        // private static bool IsDown
        // {
        //     get => GameManager.Instance.SphereElevatorState.isDown;
        //     set => GameManager.Instance.SphereElevatorState.isDown = value;
        // }
        //
        // private static bool StartOnElevator
        // {
        //     get => GameManager.Instance.SphereElevatorState.startOnElevator;
        //     set => GameManager.Instance.SphereElevatorState.startOnElevator = value;
        // }
        //
        // private void Start()
        // {
        //     foreach (var VARIABLE in COLLECTION)
        //     {
        //         
        //     }
        // }

        private void Update()
        {
            if (_playerPickupSphere.IsHoldingSphere)
            {
                _hasSphere = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Movable") && !_playerPickupSphere.IsHoldingSphere)
            {
                _hasSphere = true;
            }
        }
    }
}
