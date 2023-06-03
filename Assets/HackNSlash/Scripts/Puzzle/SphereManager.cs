using System;
using System.Collections.Generic;
using HackNSlash.ScriptableObjects;
using UnityEngine;

namespace HackNSlash.Scripts.Puzzle
{
    public class SphereManager : MonoBehaviour
    {
        [SerializeField] private Transform[] spheres;
        [SerializeField] private SphereElevatorState sphereElevatorState;

        private List<Vector3> DatabasePositions => sphereElevatorState.spherePositions;
        
        private void Awake()
        {
            SetPositionInScene();
        }

        private void SetPositionInScene()
        {
            if (DatabasePositions.Count > 0)
            {
                for (int i = 0; i < DatabasePositions.Count; i++)
                {
                    spheres[i].position = DatabasePositions[i];
                }
            }
        }
        
        public void SetPositionInDatabase()
        {
            DatabasePositions.Clear();
            
            for (int i = 0; i < spheres.Length; i++)
            {
                sphereElevatorState.spherePositions.Insert(i, spheres[i].position);
            }
        }
    }
}