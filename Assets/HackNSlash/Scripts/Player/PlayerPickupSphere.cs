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
        [SerializeField] private Transform sphere;
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
                sphere = null;
                return;
            }

            if (sphere)
            {
                sphere.GetComponent<ActivatorSphere>().isBeingHeld = isHoldingSphere;
            }
            else
            {
                sphere = closestObject;
            }
        }

        public void SphereInteract()
        {
            _playerInteraction.TrackInteractables();
            
            if (!_playerInteraction.canInteract || sphere == null)
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
            sphere.SetParent(holder);
            sphere.localPosition = Vector3.zero;
            
            // update sphere manager lists
            if (sphereElevator && sphereElevator.closestHolder) sphereManager.HolderHasSphere[sphereElevator.closestHolder.GetComponent<SphereElevatorHolder>().holderIndex] = -1;
        }

        private void DropSphere()
        {
            // placing the sphere on the elevator
            if (sphereElevator && sphereElevator.sphereToBePositioned != null)
            {
                sphere.SetParent(sphereElevator.closestHolder);
                sphere.localPosition = new Vector3(0, sphere.GetComponent<ActivatorSphere>().dropHeight, 0);
                sphere.GetComponent<ActivatorSphere>().isBeingHeld = false;
                
                // update sphere manager lists
                sphereManager.HolderHasSphere[sphereElevator.closestHolder.GetComponent<SphereElevatorHolder>().holderIndex] = 
                    sphere.GetComponent<ActivatorSphere>().sphereIndex;

                sphereElevator.sphereToBePositioned = null;

                return;
            }
            
            // placing the sphere on the ground
            if (sphere.parent == holder)
            {
                sphere.SetParent(sphereParent);
                sphere.localPosition = 
                    new Vector3(sphere.localPosition.x, sphere.GetComponent<ActivatorSphere>().dropHeight, sphere.localPosition.z);
                sphere.GetComponent<ActivatorSphere>().isBeingHeld = false;
            }

            if (!sphereElevator) return;
            
            if (sphere.GetComponent<ActivatorSphere>().isDown)
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