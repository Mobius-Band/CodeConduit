using UnityEngine;

namespace HackNSlash.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SphereElevatorState", menuName = "ScriptableObjects/SphereElevatorState", order = 1)]
    public class SphereElevatorState : ScriptableObject
    {
        public Vector3[] spherePositions;
        public float sphereAmount;
    }
}