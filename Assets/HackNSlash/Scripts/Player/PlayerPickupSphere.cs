using HackNSlash.Scripts.Puzzle;
using UnityEngine;
using UnityEngine.Serialization;

namespace HackNSlash.Scripts.Player
{
    public class PlayerPickupSphere : MonoBehaviour
    {
        [SerializeField] private SphereManager sphereManager;
        [SerializeField] private SphereElevator sphereElevator;
        [SerializeField] private Transform sphereParent;
        [SerializeField] private Transform holder;
        [HideInInspector] public bool isHoldingSphere;
        private PlayerInteraction _playerInteraction;
        private Transform _sphere;
        private Vector3[] _initialSpherePositions;
        
        private void Awake()
        {
            _playerInteraction = GetComponent<PlayerInteraction>();
        }
        
        private void Update()
        {
            isHoldingSphere = holder.childCount > 0;

            var closestObject = _playerInteraction.closestObject;
            if (closestObject == null || !closestObject.CompareTag("Movable"))
            {
                _sphere = null;
                return;
            }

            if (_sphere)
            {
                _sphere.GetComponent<ActivatorSphere>().isBeingHeld = isHoldingSphere;
            }
            else
            {
                _sphere = closestObject;
            }
        }

        public void SphereInteract()
        {
            _playerInteraction.TrackInteractables();
            
            if (!_playerInteraction.canInteract || _sphere == null)
            {
                return;
            }
            
            if (isHoldingSphere)
            {
                DropSphere();
            }
            else
            {
                PickupSphere();
            }
        }

        private void PickupSphere()
        {
            _sphere.SetParent(holder);
            _sphere.localPosition = Vector3.zero;
            // _sphere.GetComponent<ActivatorSphere>().IsOnElevator = false;
            
            // update sphere manager lists
            if (sphereElevator.closestHolder) sphereManager.HolderHasSphere[sphereElevator.closestHolder.GetComponent<SphereElevatorHolder>().holderIndex] = -1;
        }

        private void DropSphere()
        {
            // placing the sphere on the elevator
            if (sphereElevator && sphereElevator.sphereToBePositioned != null)
            {
                _sphere.SetParent(sphereElevator.closestHolder);
                _sphere.localPosition = new Vector3(0, _sphere.GetComponent<ActivatorSphere>().dropHeight, 0);
                _sphere.GetComponent<ActivatorSphere>().isBeingHeld = false;
                
                // update sphere manager lists
                sphereManager.HolderHasSphere[sphereElevator.closestHolder.GetComponent<SphereElevatorHolder>().holderIndex] = 
                    _sphere.GetComponent<ActivatorSphere>().sphereIndex;

                sphereElevator.sphereToBePositioned = null;

                return;
            }
            
            // placing the sphere on the ground
            if (_sphere.parent == holder)
            {
                _sphere.SetParent(sphereParent);
                _sphere.localPosition = 
                    new Vector3(_sphere.localPosition.x, _sphere.GetComponent<ActivatorSphere>().dropHeight, _sphere.localPosition.z);
                _sphere.GetComponent<ActivatorSphere>().isBeingHeld = false;
            }
            
            
            if (_sphere.GetComponent<ActivatorSphere>().IsDown)
            {
                sphereManager.SetPositionInDatabaseDown();
            }
            else
            {
                sphereManager.SetPositionInDatabaseUp();
            }
        }
    }
}