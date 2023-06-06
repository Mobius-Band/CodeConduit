using System.Collections.Generic;
using HackNSlash.Scripts.GameManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackNSlash.Scripts.Puzzle
{
    public class SphereManager : MonoBehaviour
    {
        [SerializeField] private Transform[] spheres;
        [SerializeField] private Transform[] sphereHolders;
        [SerializeField] private GameManager gameManager;
        public List<int> HolderHasSphere => gameManager.SphereElevatorState.holderHasSphere;
        private List<Vector3> DatabasePositionsUp => gameManager.SphereElevatorState.spherePositionsUp;
        private List<Vector3> DatabasePositionsDown => gameManager.SphereElevatorState.spherePositionsDown;
        private List<bool> SphereIsDown => gameManager.SphereElevatorState.sphereIsDown;

        private void Awake()
        {
            SetPositionInScene();
            
            if (SceneManager.GetActiveScene().name == gameManager._sphereElevatorSceneDown)
            {
                SetPositionInDatabaseDown(true);
            }
            else if (SceneManager.GetActiveScene().name == gameManager._sphereElevatorSceneUp)
            {
                SetPositionInDatabaseUp(true);
            }
        }

        private void SetPositionInScene()
        {
            // if sphere is up
            if (DatabasePositionsUp.Count > 0)
            {
                for (int i = 0; i < DatabasePositionsUp.Count; i++)
                {
                    if (!SphereIsDown[i])
                    {
                        spheres[i].position = DatabasePositionsUp[i];
                    }
                }
            }
            
            // if sphere is down
            if (DatabasePositionsDown.Count > 0)
            {
                for (int i = 0; i < DatabasePositionsDown.Count; i++)
                {
                    if (SphereIsDown[i])
                    {
                        spheres[i].position = DatabasePositionsDown[i];
                    }
                }
            }

            // if sphere is on elevator
            for (int i = 0; i < sphereHolders.Length; i++)
            {
                if (HolderHasSphere[i] != -1 && sphereHolders[i].childCount == 0)
                {
                    var sphere = spheres[HolderHasSphere[i]];
                    sphere.SetParent(sphereHolders[i]);
                    sphere.localPosition = new Vector3(0, sphere.GetComponent<ActivatorSphere>().dropHeight, 0);
                }
            }
        }

        public void SetPositionInDatabaseUp(bool addInstead = false)
        {
            DatabasePositionsUp.Clear();
            
            for (int i = 0; i < spheres.Length; i++)
            {
                if (!SphereIsDown[i] && spheres[i].gameObject.activeSelf)
                {
                    if (DatabasePositionsUp.Count < spheres.Length) addInstead = true;
                    
                    if (addInstead)
                    {
                        DatabasePositionsUp.Add(spheres[i].position);
                    }
                    else
                    {
                        DatabasePositionsUp.Insert(i, spheres[i].position);
                    }
                }
            }
        }
        
        public void SetPositionInDatabaseDown(bool addInstead = false)
        {
            DatabasePositionsDown.Clear();
            for (int i = 0; i < spheres.Length; i++)
            {
                if (SphereIsDown[i] && spheres[i].gameObject.activeSelf)
                {
                    if (DatabasePositionsDown.Count < spheres.Length) addInstead = true;
                    
                    if (addInstead)
                    {
                        DatabasePositionsDown.Add(spheres[i].position);
                    }
                    else
                    {
                        DatabasePositionsDown.Insert(i, spheres[i].position);
                    }
                }
            }
        }
    }
}