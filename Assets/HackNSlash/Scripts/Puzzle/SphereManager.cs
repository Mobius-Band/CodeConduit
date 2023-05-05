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

        private List<Vector3> databasePositions => sphereElevatorState.SpherePositions;
        
        private void Awake()
        {
            SetPositionInScene();
        }

        private void OnDisable()
        {
            databasePositions.Clear();
            Array.ForEach(spheres, SetPositionInDatabase);
        }

        private  void SetPositionInScene()
        {
            for (int i = 0; i < databasePositions.Count; i++)
            {
                spheres[i].position = databasePositions[i];
            }
        }
        
        public void SetPositionInDatabase(Transform refSphere)
        {
            var isSphereValid = false;
            int sphereIndex = 0;
            int maximumIndex = spheres.Length;
            for (sphereIndex = 0; sphereIndex < maximumIndex; sphereIndex++)
            {
                if (refSphere.Equals(spheres[sphereIndex]))
                {
                    isSphereValid = true;
                    maximumIndex = sphereIndex;
                }
            }

            sphereIndex--;
            
            if (!isSphereValid)
            {
                Debug.LogError("Sphere isn't valid!");
                return;
            }
            
            sphereElevatorState.SpherePositions.Add(refSphere.position);
            Debug.Log("sphere position ADDED");
            
            // if (databasePositions.Count < sphereIndex)
            // {
            //     databasePositions[sphereIndex] = refSphere.position;
            //     Debug.Log("sphere position CHANGE");
            // }
            // else
            // {
            //     sphereElevatorState.SpherePositions.Add(refSphere.position);
            //     Debug.Log("sphere position ADDED");
            // }
        }
    }
}