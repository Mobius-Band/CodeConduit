using UnityEngine;
using System.Collections;

public class ObjectRotationController : MonoBehaviour
{
    public Transform objectToCheck;       // The object to be rotated.
    public Transform targetObject;        // The target object to rotate towards.
    public float rotationSpeed = 90f;     // The rotation speed in degrees per second.

    private void Start()
    {
        // Start the rotation coroutine.
        StartCoroutine(RotateObjectCoroutine());
    }

    private IEnumerator RotateObjectCoroutine()
    {
        while (true)
        {
            // Calculate the direction from the objectToCheck to the targetObject.
            Vector3 targetDirection = targetObject.position - objectToCheck.position;

            // Create a rotation towards the targetDirection.
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // Calculate the rotation step based on the rotation speed and frame rate.
            float rotationStep = rotationSpeed * Time.deltaTime;

            // Rotate objectToCheck towards targetRotation.
            objectToCheck.rotation = Quaternion.RotateTowards(objectToCheck.rotation, targetRotation, rotationStep);

            yield return null;
            
            // Calculate the direction from the objectToCheck to the targetObject.
            // Vector3 targetDirection = targetObject.position - objectToCheck.position;

            // Get the angle between the objectToCheck's forward direction and the targetDirection.
            float angle = Vector3.Angle(objectToCheck.forward, targetDirection);
        }
    }
}