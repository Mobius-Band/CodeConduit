using System;
using DG.Tweening;
using HackNSlash.Scripts.GameManagement;
using HackNSlash.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackNSlash.Scripts.Puzzle
{
    public class SphereElevator : MonoBehaviour
    {
        [SerializeField] private PlayerPickupSphere _playerPickupSphere;
        [SerializeField] private PlayerInteraction _playerInteraction;
        [SerializeField] private float time;
        private Transform _sphere;
        private String _elevatorScene1 = "Part4-2-1";
        private String _elevatorScene2 = "Part4-2-2";
        private float _upPosition;
        private float _downPosition;
        private bool _hasSphere;
        
        private static bool IsDown
        {
            get => GameManager.Instance.SphereElevatorState.isDown;
            set => GameManager.Instance.SphereElevatorState.isDown = value;
        }

        private static bool StartOnElevator
        {
            get => GameManager.Instance.SphereElevatorState.startOnElevator;
            set => GameManager.Instance.SphereElevatorState.startOnElevator = value;
        }
        
        private void Start()
        {
            DOTween.Init();
            
            if (SceneManager.GetActiveScene().name == _elevatorScene1)
            {
                _upPosition = 10;
                _downPosition = 0;
            }
            else if (SceneManager.GetActiveScene().name == _elevatorScene2)
            {
                _upPosition = 0;
                _downPosition = -10;
            }
        }
        
        private void Update()
        {
            if (_playerPickupSphere.IsHoldingSphere)
            {
                _hasSphere = false;
            }

            if (_sphere && _hasSphere && !_playerPickupSphere.IsHoldingSphere)
            {
                _sphere.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Movable") && !_playerPickupSphere.IsHoldingSphere && !_hasSphere)
            {
                _hasSphere = true;
                if (_playerInteraction.ClosestObject.CompareTag("Movable"))
                {
                    _sphere = other.gameObject.transform;
                }
            }
        }

        public void ActivateButton()
        {
            if (!_playerInteraction.ClosestObject.CompareTag("Button") || _playerPickupSphere.IsHoldingSphere || !_hasSphere)
            {
                return;
            }
            
            ElevatorActivate();
        }
        
        private void ElevatorActivate()
        {
            float direction;
            if (StartOnElevator)
            {
                if (IsDown) { direction = _downPosition; }
                else { direction = _upPosition; }
            }
            else
            {
                if (IsDown) { direction = _upPosition; }
                else { direction = _downPosition; }
            }
            
            transform.DOMove(new Vector3(transform.position.x, direction, transform.position.z), time);
        }
    }
}
