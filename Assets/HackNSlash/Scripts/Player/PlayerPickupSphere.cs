using UnityEngine;

namespace HackNSlash.Scripts.Player
{
    public class PlayerPickupSphere : MonoBehaviour, IInteractor
    {
        [SerializeField] private Transform _holder; 
        [HideInInspector] public bool isHoldingSphere;
        private PlayerInteraction _playerInteraction;
        private Transform _sphere;
        private Transform _sphereParent;
        private Vector3[] _initialSpherePositions;
        public bool IsHoldingSphere => isHoldingSphere;
        
        private void Awake()
        {
            _playerInteraction = GetComponent<PlayerInteraction>();
        }

        public void Interact()
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
             if (_sphere == null)
             {
                 return;
             }
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
            isHoldingSphere = _holder.childCount > 0;

            var closestObject = _playerInteraction._closestObject;
            if (closestObject == null || !closestObject.CompareTag("Movable"))
            {
                _sphere = null;
                return;
            }
            
            _sphere = _playerInteraction._closestObject;
            if (!isHoldingSphere)
            {
                _sphereParent = _sphere.parent;
            }
            
        }
    }
}