using System.Collections.Generic;
using HackNSlash.Scripts.GameManagement;
using HackNSlash.Scripts.Player;
using UnityEngine;

namespace HackNSlash.Scripts.Puzzle
{
    public class SphereElevator : MonoBehaviour
    {
        [SerializeField] private GameObject _sphere;
        [SerializeField] private PlayerPickupSphere _playerPickupSphere;
        private bool _hasSphere;
        
        // private static float SphereAmount
        // {
        //     get => GameManager.Instance.SphereElevatorState.sphereAmount;
        //     set => GameManager.Instance.SphereElevatorState.sphereAmount = value;
        // }
        //
        // private static Dictionary<Transform, Vector3> SpherePositions
        // {
        //     get => GameManager.Instance.SphereElevatorState.SpherePositions;
        //     set => GameManager.Instance.SphereElevatorState.SpherePositions = value;
        // }
        //
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

        // private void Start()
        // {
        //     for (var i = 0; i < SphereAmount - 1; i++)
        //     {
        //         var currentSphere = Instantiate(_sphere);
        //         // currentSphere.transform.position = SpherePositions;
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
            if (other.gameObject.CompareTag("Movable") && !_playerPickupSphere.IsHoldingSphere && !_hasSphere)
            {
                _hasSphere = true;
            }
        }
    }
}
