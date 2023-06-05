using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace HackNSlash.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SphereElevatorState", menuName = "ScriptableObjects/SphereElevatorState", order = 1)]
    public class SphereElevatorState : ScriptableObject
    {
        public List<Vector3> spherePositionsUp = new();
        public List<Vector3> spherePositionsDown = new();
        public List<int> holderHasSphere = new();
        public List<bool> sphereIsDown = new();
        public bool elevatorIsDown;
    }
}