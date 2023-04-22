using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace HackNSlash.Scripts.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private float _interactionDistance;
        [SerializeField] private Transform[] _interactableObjects;
        [HideInInspector] public bool canInteract;
        private Transform _closestObject;

        private void Update()
        {
            foreach (Transform interactableObject in _interactableObjects)
            {
                if (Vector3.Distance(transform.position, interactableObject.position) < _interactionDistance)
                {
                    if (!_closestObject) _closestObject = interactableObject;
                    
                    if (Vector3.Distance(transform.position, interactableObject.position) < 
                        Vector3.Distance(transform.position, _closestObject.position))
                    {
                        _closestObject = interactableObject;
                        print(interactableObject.name);
                        canInteract = true;
                        break;
                    }
                }
                else
                {
                    canInteract = false;
                }
            }
        }
    }
}
