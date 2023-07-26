using System;
using UnityEngine;

namespace HackNSlash.Scripts.Camera
{
    public class IsometricCameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform _focus;
        [SerializeField] private float _speed;
        private Vector3 rayDirection;
        private Vector2 _input;
        private Vector2 _fixedInput;
        private float distance;
        private Vector3 _initialPosition;
        private Vector3 _initialFocusPosition;
        private Vector3 _currentFocusPosition;
        
        void Start()
        {
            if (_focus == null)
            {
                this.enabled = false;
                return;
            }
            
            _initialPosition = transform.position;
            _initialFocusPosition = _focus.position;
        }

        void Update()
        {
            _currentFocusPosition = _focus.position;
        }

        void LateUpdate()
        {
            FollowFocus();
        }

        private void FollowFocus()
        {
            Vector3 targetPosition = _currentFocusPosition + _initialPosition - _initialFocusPosition;
            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                _speed * Time.deltaTime);
        }
    }
}