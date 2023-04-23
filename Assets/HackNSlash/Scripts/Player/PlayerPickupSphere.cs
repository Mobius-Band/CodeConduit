using System;
using System.Collections.Generic;
using HackNSlash.Scripts.GameManagement;
using UnityEngine;

namespace HackNSlash.Scripts.Player
{
    public class PlayerPickupSphere : MonoBehaviour
    {
        [SerializeField] private Transform _holder; 
        [HideInInspector] public bool isHoldingSphere;
        private PlayerInteraction _playerInteraction;
        private Transform _sphere;
        private Transform _sphereParent;
        private Vector3[] _initialSpherePositions;
            public bool IsHoldingSphere => isHoldingSphere;

        private static Dictionary<Transform, Vector3> SpherePositions
        {
            get => GameManager.Instance.SphereElevatorState.SpherePositions;
            set => GameManager.Instance.SphereElevatorState.SpherePositions = value;
        }
        
        private void Awake()
        {
            _playerInteraction = GetComponent<PlayerInteraction>();
            // _initialSpherePositions = SpherePositions;
        }

        public void SphereInteract()
        {
            if (_playerInteraction.canInteract)
            {
                if (!isHoldingSphere)
                {
                    PickupSphere();
                }
                else
                {
                    DropSphere();
                }
            }
        }

        private void PickupSphere()
         {
            _sphere.SetParent(_holder);
            _sphere.localPosition = Vector3.zero;
                    
            _sphere.GetComponent<Collider>().isTrigger = true;
        }

        private void DropSphere()
        {
            _sphere.SetParent(_sphereParent);
            _sphere.localPosition = new Vector3(_sphere.localPosition.x, 1, _sphere.localPosition.z);

            _sphere.GetComponent<Collider>().isTrigger = false;

            SetSpherePosition();
        }

        private void Update()
        {
            if (_holder.childCount > 0)
            {
                isHoldingSphere = true;
            }
            else
            {
                isHoldingSphere = false;
            }
            
            if (_playerInteraction._closestObject == null)
            {
                return;
            }
            
            if (_playerInteraction._closestObject.CompareTag("Movable"))
            {
                _sphere = _playerInteraction._closestObject;
                
                if (!isHoldingSphere)
                {
                    _sphereParent = _sphere.parent;
                }
            }
        }

        private void SetSpherePosition()
        {
            
        }
    }
}