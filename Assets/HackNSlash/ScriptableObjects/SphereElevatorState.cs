using System.Collections.Generic;
using UnityEngine;

namespace HackNSlash.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SphereElevatorState", menuName = "ScriptableObjects/SphereElevatorState", order = 1)]
    public class SphereElevatorState : ScriptableObject
    {
        public List<Vector3> spherePositions = new();
        public float sphereAmount;
        public bool isDown;
        public bool startOnElevator = false;

        private void OnEnable()
        {
            spherePositions.Clear();
        }
    }
}