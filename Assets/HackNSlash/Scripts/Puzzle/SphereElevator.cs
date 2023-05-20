using HackNSlash.Scripts.Player;
using UnityEngine;

namespace HackNSlash.Scripts.Puzzle
{
    public class SphereElevator : MonoBehaviour
    {
        [SerializeField] private Transform button;
        [SerializeField] private PlayerPickupSphere _playerPickupSphere;
        [SerializeField] private PlayerInteraction _playerInteraction;
        private Transform _sphere;
        private bool _hasSphere;
        
        private void Update()
        {
            if (_playerPickupSphere.IsHoldingSphere)
            {
                _hasSphere = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Movable") && !_playerPickupSphere.IsHoldingSphere && !_hasSphere)
            {
                _hasSphere = true;
                if (_playerInteraction.ClosestObject.CompareTag("Movable"))
                {
                    _sphere = other.gameObject.transform;
                    print(_sphere.name);
                }
            }
        }

        public void ActivateButton()
        {
            if (!_playerInteraction.ClosestObject.CompareTag("Button") || _playerPickupSphere.IsHoldingSphere || !_hasSphere)
            {
                return;
            }
            
            print("activated the elevator");
        }
    }
}
