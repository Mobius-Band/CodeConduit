using System;
using System.Collections.Generic;
using DG.Tweening;
using HackNSlash.Scripts.GameManagement;
using HackNSlash.Scripts.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace HackNSlash.Scripts.Puzzle
{
    public class SphereElevator : MonoBehaviour
    {
        [HideInInspector] public Transform closestHolder;
        [SerializeField] private Transform player;
        [SerializeField] private Transform[] sphereHolders;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private PlayerInteraction playerInteraction;
        [SerializeField] private float upPosition;
        [SerializeField] private float downPosition;
        [SerializeField] private float travelTime;
        [SerializeField] private float minimumHolderPlayerDistance;
        private bool _sphereIsTouchingElevator;
        private PlayerPickupSphere _playerPickupSphere;
        private List<int> HolderHasSphere => gameManager.SphereElevatorState.holderHasSphere;
        private const float BigNumber = 10000;
        private bool _isMoving;
        private bool _canActivateElevator;
        private float _closestHolderDistance;

        private bool IsDown
        {
            get => gameManager.SphereElevatorState.elevatorIsDown;
            set => gameManager.SphereElevatorState.elevatorIsDown = value;
        }

        private void Start()
        {
            var startPosition = IsDown ? downPosition : upPosition;
            transform.position = new Vector3(0, startPosition, 0);

            _playerPickupSphere = player.GetComponent<PlayerPickupSphere>();
            _closestHolderDistance = BigNumber;

            DOTween.Init();
        }

        private void Update()
        {
            if (_playerPickupSphere.isHoldingSphere)
            {
                CheckForClosestHolder();
                SetHolderMesh();
            }

            foreach (var holder in sphereHolders)
            {
                if (holder.childCount > 0)
                {
                    holder.GetChild(0).gameObject.SetActive(true);
                }
            }

            var closestObject = playerInteraction.ClosestObject;
            if (!closestObject)
            {
                _canActivateElevator = false;
                return;
            }

            _canActivateElevator = playerInteraction.ClosestObject.CompareTag("Button");
        }

        public void ElevatorActivate()
        {
            if (!_canActivateElevator) return;

            var direction = IsDown ? upPosition : downPosition;

            _isMoving = true;
            transform.DOMove(new Vector3(transform.position.x, direction, transform.position.z), travelTime);

            // set elevator and spheres isDown value
            IsDown = !IsDown;

            for (int i = 0; i < sphereHolders.Length; i++)
            {
                if (sphereHolders[i].childCount > 0)
                {
                    gameManager.SphereElevatorState.sphereIsDown[
                        sphereHolders[i].GetChild(0).GetComponent<ActivatorSphere>().sphereIndex] = IsDown;
                    HolderHasSphere[i] = sphereHolders[i].GetChild(0).GetComponent<ActivatorSphere>().sphereIndex;
                }
                else
                {
                    HolderHasSphere[i] = -1;
                }
            }
        }

        public bool canPositionSphereOnHolder()
        {
            if (!closestHolder) return false;
            if (closestHolder.childCount > 0) return false;

            return _playerPickupSphere.isHoldingSphere && _sphereIsTouchingElevator;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Movable"))
            {
                if (other.gameObject.GetComponent<ActivatorSphere>().isBeingHeld && closestHolder &&
                    closestHolder.childCount == 0)
                {
                    _sphereIsTouchingElevator = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Movable"))
            {
                if (other.gameObject.GetComponent<ActivatorSphere>().isBeingHeld)
                {
                    _sphereIsTouchingElevator = false;
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

            if (closestHolder &&
                Vector3.Distance(player.position, closestHolder.position) > minimumHolderPlayerDistance)
            {
                closestHolder = null;
            }
        }

        private void SetHolderMesh()
        {
            if (!closestHolder) return;

            if (canPositionSphereOnHolder())
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

            if (!canPositionSphereOnHolder())
            {
                foreach (var holder in sphereHolders)
                {
                    holder.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
        // holder debug
        // private void OnDrawGizmos()
        // {
        //     if (player && closestHolder)
        //     {
        //         Gizmos.color = Color.magenta;
        //         foreach (var holder in sphereHolders)
        //         {
        //             Gizmos.DrawLine(player.position, holder.position);
        //         }
        //         Gizmos.color = Color.green;
        //         Gizmos.DrawLine(player.position, closestHolder.position);
        //     }
        // }
    }
}
