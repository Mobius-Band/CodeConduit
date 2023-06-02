using System;
using DG.Tweening;
using HackNSlash.Scripts.GameManagement;
using HackNSlash.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace HackNSlash.Scripts.Puzzle
{
    public class SphereElevator : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private Transform[] sphereHolders;
        [SerializeField] private float upPosition;
        [SerializeField] private float downPosition;
        [SerializeField] private float travelTime;
        [SerializeField] private float minimumHolderPlayerDistance;
        public bool canPressButton = false;
        private PlayerPickupSphere _playerPickupSphere;
        private PlayerInteraction _playerInteraction;
        private Transform Sphere => _playerPickupSphere.sphere;
        private Transform _closestHolder;
        private const float BigNumber = 10000;
        private bool _sphereIsOnElevator;
        private bool _canPositionSphereOnHolder;
        /*private*/ public float _closestHolderDistance;
        
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
            _playerPickupSphere = player.GetComponent<PlayerPickupSphere>();
            _playerInteraction = player.GetComponent<PlayerInteraction>();
            
            DOTween.Init();

            _closestHolderDistance = BigNumber;
        }
        
        private void Update()
        {
            if (_playerPickupSphere.IsHoldingSphere)
            {
                _sphereIsOnElevator = false;
                
                CheckForClosestHolder();

                // enable/disable sphere holder mesh
                if (_closestHolder)
                {
                    _closestHolder.GetComponent<MeshRenderer>().enabled = true;
                    
                    foreach (var holder in sphereHolders)
                    {
                        if (holder != _closestHolder)
                        {
                            holder.GetComponent<MeshRenderer>().enabled = false;
                        }
                    }
                }
                else
                {
                    foreach (var holder in sphereHolders)
                    {
                        holder.GetComponent<MeshRenderer>().enabled = false;
                    }
                }
            }

            if (Sphere && _sphereIsOnElevator && !_playerPickupSphere.IsHoldingSphere && _closestHolder)
            {
                Sphere.position = new Vector3(_closestHolder.position.x, _closestHolder.position.y + 1, _closestHolder.position.z);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Movable"))
            {
                if (!_canPositionSphereOnHolder)
                {
                    _canPositionSphereOnHolder = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Movable"))
            {
                if (_playerPickupSphere.IsHoldingSphere)
                {
                    _canPositionSphereOnHolder = false;
                }
            }
        }

        public void ActivateButton()
        {
            if (!_playerInteraction.ClosestObject.CompareTag("Button") || _playerPickupSphere.IsHoldingSphere)
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
                if (IsDown) { direction = downPosition; }
                else { direction = upPosition; }
            }
            else
            {
                if (IsDown) { direction = upPosition; }
                else { direction = downPosition; }
            }
            
            transform.DOMove(new Vector3(transform.position.x, direction, transform.position.z), travelTime);
        }

        private void CheckForClosestHolder()
        {
            if (_closestHolder)
            {
                _closestHolderDistance = Vector3.Distance(_closestHolder.position, player.position);
            }
            else
            {
                _closestHolderDistance = BigNumber;
            }
            
            foreach (var holder in sphereHolders)
            {
                if (Vector3.Distance(holder.position, player.position) < _closestHolderDistance)
                {
                    _closestHolder = holder;
                }
            }
            
            if (_closestHolder && Vector3.Distance(player.position, _closestHolder.position) > minimumHolderPlayerDistance)
            {
                _closestHolder = null;
            }
        }

        // private void OnDrawGizmos()
        // {
        //     if (player && _closestHolder)
        //     {
        //         Gizmos.color = Color.magenta;
        //         foreach (var holder in sphereHolders)
        //         {
        //             Gizmos.DrawLine(player.position, holder.position);
        //         }
        //         Gizmos.color = Color.green;
        //         Gizmos.DrawLine(player.position, _closestHolder.position);
        //     }
        // }
    }
}
