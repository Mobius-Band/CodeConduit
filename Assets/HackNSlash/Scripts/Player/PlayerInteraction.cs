using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace HackNSlash.Scripts.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private float _interactionDistance;
        [SerializeField] private Transform[] _interactableObjects;
        [HideInInspector] public Transform _closestObject;
        [HideInInspector] public bool canInteract;

        private void Update()
        {
            foreach (var interactableObject in _interactableObjects)
            {
                if (Vector3.Distance(transform.position, interactableObject.position) < _interactionDistance)
                {
                    if (!_closestObject) _closestObject = interactableObject;
                    canInteract = true;
                    
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
                canInteract = false;
            }
        }
    }
}
