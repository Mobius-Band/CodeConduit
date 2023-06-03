using HackNSlash.Scripts.Puzzle;
using UnityEngine;
using UnityEngine.Serialization;

namespace HackNSlash.Scripts.Player
{
    public class PlayerPickupSphere : MonoBehaviour
    {
        [SerializeField] private SphereManager sphereManager;
        [SerializeField] private SphereElevator sphereElevator;
        [SerializeField] private Transform holder;
        [HideInInspector] public Transform sphere;
        [HideInInspector] public bool isHoldingSphere;
        public float dropHeight;
        private PlayerInteraction _playerInteraction;
        private Transform _sphereParent;
        private Vector3[] _initialSpherePositions;
        
        private void Awake()
        {
            _playerInteraction = GetComponent<PlayerInteraction>();
        }

        public void SphereInteract()
        {
            if (!_playerInteraction.CanInteract)
            {
                return;
            }
            
            if (!isHoldingSphere)
            {
                PickupSphere();
            }
            else
            {
                DropSphere();
            }
        }

        private void PickupSphere()
        {
            if (sphere == null) return;
             
            sphere.SetParent(holder);
            sphere.localPosition = Vector3.zero;

            sphere.GetComponent<ActivatorSphere>().isBeingHeld = true;
        }

        private void DropSphere()
        {
            if (sphere == null) return;
             
            sphere.SetParent(_sphereParent);
            sphere.localPosition = new Vector3(sphere.localPosition.x, dropHeight, sphere.localPosition.z);

            // placing the sphere on the elevator
            if (sphereElevator && sphereElevator.canPositionSphereOnHolder)
            {
                sphere.SetParent(sphereElevator.closestHolder);
                sphere.localPosition = new Vector3(0, dropHeight, 0);
            }

            sphere.GetComponent<ActivatorSphere>().isBeingHeld = false;
            if (sphereManager) sphereManager.SetPositionInDatabase();
        }

        private void Update()
        {
            isHoldingSphere = holder.childCount > 0;

            var closestObject = _playerInteraction.ClosestObject;
            if (closestObject == null || !closestObject.CompareTag("Movable"))
            {
                sphere = null;
                return;
            }

            if (sphere)
            {
                if (!isHoldingSphere)
                {
                    _sphereParent = sphere.parent;
                }
            }
            else
            {
                sphere = _playerInteraction.ClosestObject;
            }
                
        }
    }
}