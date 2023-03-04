using System;
using DG.Tweening;
using UnityEngine;

namespace HackNSlash.Scripts.Player
{
    public class PlayerPickupSphere : MonoBehaviour
    {
        [SerializeField] private float _distance;
        [SerializeField] private float _height;
        private Transform _sphere;
        private Collider _collider;
        private bool _canPickUp = true;
        private bool _isHoldingSphere = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Movable"))
            {
                _sphere = other.transform;
                if (!_isHoldingSphere)
                {
                    _canPickUp = true;
                    print("can pickup");
                }
            }
        }

        public void PickupSphere()
        {
            if (_canPickUp)
            {
                _isHoldingSphere = true;
                _canPickUp = false;
            }
        }

        private void Update()
        {
            if (_isHoldingSphere && _sphere != null)
            {
                _sphere.transform.position = new Vector3(transform.position.x, transform.position.y + _height, transform.position.z + _distance);
            }
        }
    }
}