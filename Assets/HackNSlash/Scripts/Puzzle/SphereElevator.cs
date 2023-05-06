using HackNSlash.Scripts.Player;
using UnityEngine;

namespace HackNSlash.Scripts.Puzzle
{
    public class SphereElevator : MonoBehaviour
    {
        // [SerializeField] private GameObject _sphere;
        [SerializeField] private PlayerPickupSphere _playerPickupSphere;
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
            }
        }
    }
}
