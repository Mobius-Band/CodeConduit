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

            var closestObject = _playerInteraction.ClosestObject;
            if (closestObject == null || !closestObject.CompareTag("Movable"))
            {
                _sphere = null;
                return;
            }

            if (_sphere)
            {
                if (isHoldingSphere)
                {
                    _sphere.GetComponent<ActivatorSphere>().isBeingHeld = true;
                }
                else
                {
                    _sphere.GetComponent<ActivatorSphere>().isBeingHeld = false;
                }
            }
            else
            {
                _sphere = closestObject;
            }
        }

        public void SphereInteract()
        {
            _playerInteraction.TrackInteractables();
            
            if (!_playerInteraction.CanInteract || _sphere == null)
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
            if (sphereElevator && sphereElevator.canPositionSphereOnHolder())
            {
                _sphere.SetParent(sphereElevator.closestHolder);
                _sphere.localPosition = new Vector3(0, _sphere.GetComponent<ActivatorSphere>().dropHeight, 0);
                
                // update sphere manager lists
                sphereManager.HolderHasSphere[sphereElevator.closestHolder.GetComponent<SphereElevatorHolder>().holderIndex] = _sphere.GetComponent<ActivatorSphere>().sphereIndex;
            }
            
            // placing the sphere on the ground
            if (_sphere.parent == holder)
            {
                _sphere.SetParent(sphereParent);
                _sphere.localPosition = new Vector3(_sphere.localPosition.x, _sphere.GetComponent<ActivatorSphere>().dropHeight, _sphere.localPosition.z);
            }
            
            _sphere.GetComponent<ActivatorSphere>().isBeingHeld = false;
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