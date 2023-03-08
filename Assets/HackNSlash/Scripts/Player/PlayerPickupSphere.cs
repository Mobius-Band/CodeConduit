using UnityEngine;
using UnityEngine.Events;

namespace HackNSlash.Scripts.Player
{
    public class PlayerPickupSphere : MonoBehaviour
    {
        [Header("SETUP")]
        [SerializeField] private Transform _holder;
        [SerializeField] private float _activationDistance;
        [SerializeField] private bool _canPickUp;
        [SerializeField] private bool _isHoldingSphere;
        [Header("EVENTS")] 
        [SerializeField] private UnityEvent<bool> OnSphereCanBePicked;
         private Transform _sphere;
         private Transform _sphereParent;
         private BoxCollider _collider;
         private float _colliderZInitialValue;
         
         public bool IsHoldingSphere => _isHoldingSphere;

         private void Start()
         {
             _collider = GetComponent<BoxCollider>();
             _colliderZInitialValue = _collider.size.z;
         }

         public void PickupSphere()
         {
            if (_isHoldingSphere)
            {
                _sphere.GetComponent<Collider>().isTrigger = false;
                DropSphere();
            }
            
            if (_canPickUp && !_isHoldingSphere)
            {
                _sphere.GetComponent<Collider>().isTrigger = true;
                _isHoldingSphere = true;
            }
        }

        private void DropSphere()
        {
            _sphere.SetParent(_sphereParent);
            _sphere.localPosition = new Vector3(_sphere.localPosition.x, 1, _sphere.localPosition.z);

            _isHoldingSphere = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Movable"))
            {
                _sphere = other.gameObject.transform;
                if (!_isHoldingSphere)
                {
                    _sphereParent = _sphere.parent;
                }
            }
        }

        private void Update()
        {
            if (_sphere == null)
            {
                return;
            }
            
            if (Vector3.Distance(transform.position, _sphere.position) < _activationDistance)
            {
                if (_canPickUp == false && _isHoldingSphere == false)
                {
                    OnSphereCanBePicked?.Invoke(!_canPickUp);
                }
                _canPickUp = true;
            }
            else
            {
                if (_canPickUp == true)
                {
                    OnSphereCanBePicked?.Invoke(!_canPickUp);
                }
                _canPickUp = false;
            }
            
            var colliderSize = _collider.size;
            if (_isHoldingSphere)
            {
                _sphere.SetParent(_holder);
                _sphere.localPosition = Vector3.zero;
                colliderSize.z = 4;
            }
            else
            {
                colliderSize.z = _colliderZInitialValue;
            }

            _collider.size = colliderSize;
        }
    }
}