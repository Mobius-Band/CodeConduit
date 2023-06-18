using System;
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

        public void Reset()
        {
            spherePositionsUp.Clear();
            spherePositionsDown.Clear();
            ResetHolderState();
            ResetSphereState();
            elevatorIsDown = true;
        }

        private void ResetHolderState()
        {
            holderHasSphere = new List<int>(4);
            for (int i = 0; i < holderHasSphere.Capacity; i++)
            {
                holderHasSphere.Add(-1);
            }
        }

        private void ResetSphereState()
        {
            sphereIsDown = new List<bool>(3)
            {
                true,
                false,
                false
            };
        }
    }
}