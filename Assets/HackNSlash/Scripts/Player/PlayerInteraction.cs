using System;
using System.Collections;
using UnityEngine;

namespace HackNSlash.Scripts.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("TRACKING")] 
        [SerializeField] private float trackingDistance = 9999f;
        [SerializeField] LayerMask trackingMask;
        private Transform[] _interactableObjects;

        [Header("INTERACTION")]
        [SerializeField] private float _interactionDistance;
        private Transform _closestObject;
        private bool _canInteract;
        public bool CanInteract => _canInteract;
        public Transform ClosestObject => _closestObject;

        private void Start()
        {
            TrackInteractables();
        }

        private void TrackInteractables()
        {
            var colliders = Physics.OverlapSphere(transform.position, trackingDistance, trackingMask, QueryTriggerInteraction.Collide);
            _interactableObjects = new Transform[colliders.Length];
            for (int i = 0; i < colliders.Length; i++)
            {
                _interactableObjects[i] = colliders[i].transform;
            }
        }

        private void Update()
        {
            foreach (var interactableObject in _interactableObjects)
            {
                if (Vector3.Distance(transform.position, interactableObject.position) < _interactionDistance)
                {
                    if (!_closestObject) _closestObject = interactableObject;
                    _canInteract = true;
                    
                    if (Vector3.Distance(transform.position, interactableObject.position) < 
                        Vector3.Distance(transform.position, _closestObject.position))
                    {
                        _closestObject = interactableObject;
                        break;
                    }
                }
            }

            if (!_closestObject)
            {
                return;
            }
            
            if (Vector3.Distance(transform.position, _closestObject.position) > _interactionDistance)
            {
                _canInteract = false;
            }
        }
    }
}
