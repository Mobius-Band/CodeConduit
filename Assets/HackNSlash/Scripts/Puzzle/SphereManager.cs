using System;
using HackNSlash.ScriptableObjects;
using UnityEngine;

namespace HackNSlash.Scripts.Puzzle
{
    public class SphereManager : MonoBehaviour
    {
        [SerializeField] private Transform[] spheres;
        [SerializeField] private SphereElevatorState sphereElevatorState;

        private void Start()
        {
            SetPositionInScene();
        }

        private  void SetPositionInScene()
        {
            foreach (var sphere in spheres)
            {
                if (sphereElevatorState.SpherePositions.TryGetValue(sphere, out var databasePosition))
                {
                    sphere.position = databasePosition;
                }
            }
        }
        
        public void SetPositionInDatabase(Transform refSphere)
        {
            var isSphereValid = false;
            foreach (var sphere in spheres)
            {
                if (refSphere.Equals(sphere))
                {
                    isSphereValid = true;
                }
            }
            
            if (!isSphereValid)
            {
                Debug.LogError("Sphere isn't valid!");
                return;
            }

            if (sphereElevatorState.SpherePositions.ContainsKey(refSphere))
            {
                sphereElevatorState.SpherePositions[refSphere] = refSphere.position;
                Debug.Log("sphere position CHANGE");
            }
            else
            {
                sphereElevatorState.SpherePositions.Add(refSphere, refSphere.position);
                Debug.Log("sphere position ADDED");
            }
            
            
        }
    }
}