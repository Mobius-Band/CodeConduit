using UnityEngine;
using UnityEngine.Serialization;

namespace HackNSlash.Scripts.Player
{
    public class PlayerPickupSphere : MonoBehaviour
    {
        [SerializeField] private Transform holder;
        [SerializeField] private float dropHeight;
        [HideInInspector] public bool isHoldingSphere;
        [HideInInspector] public Transform sphere;
        private PlayerInteraction _playerInteraction;
        private Transform _sphereParent;
        private Vector3[] _initialSpherePositions;
        public bool IsHoldingSphere => isHoldingSphere;
        
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
             if (sphere == null)
             {
                 return;
             }
             sphere.SetParent(holder);
            sphere.localPosition = Vector3.zero;
                    
            sphere.GetComponent<Collider>().isTrigger = true;
        }

        private void DropSphere()
        {
            sphere.SetParent(_sphereParent);
            sphere.localPosition = new Vector3(sphere.localPosition.x, dropHeight, sphere.localPosition.z);

            sphere.GetComponent<Collider>().isTrigger = false;
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
            
            sphere = _playerInteraction.ClosestObject;
            if (!isHoldingSphere)
            {
                _sphereParent = sphere.parent;
            }
            
        }
    }
}