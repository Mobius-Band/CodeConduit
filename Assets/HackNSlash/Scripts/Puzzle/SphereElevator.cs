using System;
using System.Collections.Generic;
using DG.Tweening;
using HackNSlash.Scripts.GameManagement;
using HackNSlash.Scripts.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace HackNSlash.Scripts.Puzzle
{
    public class SphereElevator : MonoBehaviour
    {
        [HideInInspector] public Transform closestHolder;
        public List<int> spheresOnElevator = new();
        [HideInInspector] public bool canPositionSphereOnHolder;
        [SerializeField] private Transform player;
        [SerializeField] private Transform[] sphereHolders;
        [SerializeField] private SphereElevatorButton sphereElevatorButton;
        [SerializeField] private float upPosition;
        [SerializeField] private float downPosition;
        [SerializeField] private float travelTime;
        [SerializeField] private float minimumHolderPlayerDistance;
        private PlayerPickupSphere _playerPickupSphere;
        private Transform Sphere => _playerPickupSphere.sphere;
        private const float BigNumber = 10000;
        private bool _isMoving;
        private float _closestHolderDistance;
        
        private static bool IsDown
        {
            get => GameManager.Instance.SphereElevatorState.isDown;
            set => GameManager.Instance.SphereElevatorState.isDown = value;
        }

        private void Start()
        {
            _playerPickupSphere = player.GetComponent<PlayerPickupSphere>();
            
            DOTween.Init();

            _closestHolderDistance = BigNumber;
        }
        
        private void Update()
        {
            if (_playerPickupSphere.isHoldingSphere)
            {
                CheckForClosestHolder();

                // enable/disable sphere holder mesh
                if (closestHolder)
                {
                    if (closestHolder.childCount > 0)
                    {
                        canPositionSphereOnHolder = false;
                    }
                    
                    if (canPositionSphereOnHolder)
                    {
                        closestHolder.GetComponent<MeshRenderer>().enabled = true;
                    }
                    
                    foreach (var holder in sphereHolders)
                    {
                        if (holder != closestHolder)
                        {
                            holder.GetComponent<MeshRenderer>().enabled = false;
                        }
                    }
                }
                else
                {
                    canPositionSphereOnHolder = false;
                }
                
                if (!closestHolder || !canPositionSphereOnHolder)
                {
                    foreach (var holder in sphereHolders)
                    {
                        holder.GetComponent<MeshRenderer>().enabled = false;
                    }
                }
            }
            else
            {
                canPositionSphereOnHolder = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Movable"))
            {
                if (other.gameObject.GetComponent<ActivatorSphere>().isBeingHeld && closestHolder && closestHolder.childCount == 0)
                {
                    canPositionSphereOnHolder = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Movable"))
            {
                if (other.gameObject.GetComponent<ActivatorSphere>().isBeingHeld)
                {
                    canPositionSphereOnHolder = false;
                }
            }
        }

        public void ElevatorActivate()
        {
            if (_isMoving) return;
            
            float direction;
            
            if (IsDown) { direction = upPosition; }
            else { direction = downPosition; }

            _isMoving = true;
            transform.DOMove(new Vector3(transform.position.x, direction, transform.position.z), travelTime);

            foreach (var holder in sphereHolders)
            {
                if (holder.childCount > 0)
                {
                    holder.GetChild(0).GetComponent<ActivatorSphere>().isOnElevator = true;
                }
            }
        }

        private void CheckForClosestHolder()
        {
            foreach (var holder in sphereHolders)
            {
                if (Vector3.Distance(holder.position, player.position) < _closestHolderDistance)
                {
                    closestHolder = holder;
                }
            }
            
            if (closestHolder)
            {
                _closestHolderDistance = Vector3.Distance(closestHolder.position, player.position);
            }
            else
            {
                _closestHolderDistance = BigNumber;
            }
            
            if (closestHolder && Vector3.Distance(player.position, closestHolder.position) > minimumHolderPlayerDistance)
            {
                closestHolder = null;
            }
        }

        private void OnDrawGizmos()
        {
            if (player && closestHolder)
            {
                Gizmos.color = Color.magenta;
                foreach (var holder in sphereHolders)
                {
                    Gizmos.DrawLine(player.position, holder.position);
                }
                Gizmos.color = Color.green;
                Gizmos.DrawLine(player.position, closestHolder.position);
            }
        }
    }
}
