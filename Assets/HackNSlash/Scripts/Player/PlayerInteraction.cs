using UnityEngine;

namespace HackNSlash.Scripts.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("TRACKING")] 
        [SerializeField] private float trackingDistance = 9999f;
        [SerializeField] private LayerMask trackingMask;
        [SerializeField] private Transform[] interactableObjects;

        [Header("INTERACTION")]
        [SerializeField] private float interactionDistance;
        [HideInInspector] public bool canInteract;
        [HideInInspector] public Transform closestObject;
        
        private void Start()
        {
            TrackInteractables();
        }

        public void TrackInteractables()
        {
            var colliders = Physics.OverlapSphere(transform.position, trackingDistance, trackingMask, QueryTriggerInteraction.Collide);
            interactableObjects = new Transform[colliders.Length];
            for (int i = 0; i < colliders.Length; i++)
            {
                interactableObjects[i] = colliders[i].transform;
            }
        }

        private void Update()
        {
            foreach (var interactableObject in interactableObjects)
            {
                if (Vector3.Distance(transform.position, interactableObject.position) < interactionDistance)
                {
                    if (!closestObject) closestObject = interactableObject;
                    canInteract = true;
                    
                    if (Vector3.Distance(transform.position, interactableObject.position) < 
                        Vector3.Distance(transform.position, closestObject.position))
                    {
                        break;
                    }
                }
            }

            if (!closestObject) return;
            
            if (Vector3.Distance(transform.position, closestObject.position) > interactionDistance)
            {
                closestObject = null;
                canInteract = false;
            }
        }
    }
}
