using System;
using UnityEngine;
using UnityEngine.Events;

namespace HackNSlash.Scripts.Player
{
    public class PlayerPickupSphere : MonoBehaviour
    {
        [SerializeField] private Transform _holder; 
        private PlayerInteraction _playerInteraction;
        private Transform _sphere;
        private bool _isHoldingSphere;
        private Transform _sphereParent;
        public bool IsHoldingSphere => _isHoldingSphere;

        private void Awake()
        {
            _playerInteraction = GetComponent<PlayerInteraction>();
        }

        public void SphereInteract()
        {
            if (_playerInteraction.canInteract)
            {
                if (!_isHoldingSphere)
                {
                    PickupSphere();
                }
                else
                {
                    DropSphere();
                }
            }
        }
        
         public void PickupSphere()
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
        }

        private void Update()
        {
            if (_holder.childCount > 0)
            {
                _isHoldingSphere = true;
            }
            else
            {
                _isHoldingSphere = false;
            }
            
            if (_playerInteraction._closestObject == null)
            {
                return;
            }
            
            if (_playerInteraction._closestObject.CompareTag("Movable"))
            {
                _sphere = _playerInteraction._closestObject;
                
                if (!_isHoldingSphere)
                {
                    _sphereParent = _sphere.parent;
                }
            }
        }
    }
}