using System;
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
        private Animator _animator;
        public int sphereIndex;
        public float dropHeight;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            
            bool _isDown = gameManager.SphereElevatorState.sphereIsDown[sphereIndex];
            isDown = _isDown;
            if (SceneManager.GetActiveScene().name == gameManager.sphereElevatorSceneDown.Name)
            {
                gameObject.SetActive(isDown);
            }
            
            if (SceneManager.GetActiveScene().name == gameManager.sphereElevatorSceneUp.Name)
            {
                gameObject.SetActive(!isDown);
            }
        }

        private void Update()
        {
            _animator.SetBool("isBeingHeld", isBeingHeld);
        }
    }
}
