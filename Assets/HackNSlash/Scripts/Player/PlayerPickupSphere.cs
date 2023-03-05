using System;

using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace HackNSlash.Scripts.Player
{
    public class PlayerPickupSphere : MonoBehaviour
    {
        [SerializeField] private float _activationDistance;
        [SerializeField] private Transform _holder;
        //[SerializeField] private float _distance;
        //[SerializeField] private float _height;
        [SerializeField] private Transform _sphere;
        [SerializeField] private Transform _sphereParent;
        [SerializeField] private bool _canPickUp;
        [SerializeField] private bool _isHoldingSphere;

        public void PickupSphere()
        {
            if (_isHoldingSphere)
            {
                DropSphere();
            }
            
            if (_canPickUp && !_isHoldingSphere)
            {
                _isHoldingSphere = true;
            }
        }

        private void DropSphere()
        {
            _isHoldingSphere = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Movable"))
            {
                _sphere = other.gameObject.transform;
                /*
                if (!_isHoldingSphere)
                {
                    _sphereParent = _sphere.parent;
                }
                */
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
                _canPickUp = true;
            }
            else
            {
                _canPickUp = false;
            }
            
            if (_isHoldingSphere)
            {
                _sphere.SetParent(_holder);
                _sphere.localPosition = Vector3.zero;
                _sphere.GetComponent<Rigidbody>().useGravity = false;
            }
            else
            {
                _sphere.SetParent(_sphereParent);
                _sphere.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }
}