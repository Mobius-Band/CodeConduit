using HackNSlash.Scripts.GameManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackNSlash.Scripts.Puzzle
{
    public class ActivatorSphere : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [HideInInspector] public bool isBeingHeld;
        [HideInInspector] public bool isDown;
        public int sphereIndex;
        public float dropHeight;

        private void Awake()
        {
            bool _isDown = gameManager.SphereElevatorState.sphereIsDown[sphereIndex];
            isDown = _isDown;
            if (SceneManager.GetActiveScene().name == gameManager.sphereElevatorSceneDown)
            {
                gameObject.SetActive(isDown);
            }
            
            if (SceneManager.GetActiveScene().name == gameManager.sphereElevatorSceneUp)
            {
                gameObject.SetActive(!isDown);
            }
        }
    }
}
