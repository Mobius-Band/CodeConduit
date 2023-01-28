using System;
using System.Collections;
using Combat;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Range(1, 100)] 
        [SerializeField] private float _moveSpeed;
        [Range(1, 50)] 
        [SerializeField] private float _rotationTime = 1f;
        // [SerializeField] private Transform _perspectiveCameraHolder;
        // [SerializeField] private Transform _isometricCameraHolder;
        [SerializeField] private Transform _cameraHolder;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _dashSpeed;
        [SerializeField] private float _dashTime;
        public Vector2 MoveInput { get => _moveInput; set => _moveInput = value; }
        private ComboManager _comboManager;
        private Rigidbody _rigidbody;
        private Vector2 _moveInput;
        // private Vector2 _rotationInput;
        private Vector3 _moveDirection;
        private float _rotationVelocity;
        private bool _isDashing;
        private bool _isMovementSuspended;
        private bool _isRotationSuspended;
        private float _rotationAngle;
        private float movementAngle;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _comboManager = GetComponent<ComboManager>();
        }

        private void Update()
        {
            DefineMovementAngle(out movementAngle, ref _moveDirection);

            if (_moveInput == Vector2.zero || _isRotationSuspended)
            {
                return;
            }
            
            LerpRotate(movementAngle);
        }

        private void FixedUpdate()
        {
            if (IsMoving())
            {
                if (_animator != null) _animator.SetBool("isMoving", true);
                _rigidbody.velocity = _moveDirection * _moveSpeed;
            }
            else
            {
                if (_animator != null) _animator.SetBool("isMoving", false);
            }
        }
        
        private void LerpRotate(float movementAngle)
        {
            _rotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, movementAngle, ref _rotationVelocity,
                _rotationTime / 100);
            transform.rotation = Quaternion.Euler(0f, _rotationAngle, 0f);
        }

        private void DefineMovementAngle(out float movementAngle, ref Vector3 moveDirection)
        {
            movementAngle = Mathf.Atan2(_moveInput.x, _moveInput.y) * Mathf.Rad2Deg + _cameraHolder.eulerAngles.y;

            moveDirection = Quaternion.Euler(0f, movementAngle, 0f) * Vector3.forward;
            moveDirection.Normalize();
        }
        
        public void Dash()
        {
            if (!_isDashing)
            {
                StartCoroutine(DashCoroutine());
            }
        }

        private IEnumerator DashCoroutine()
        {
            var direction = transform.forward;
            if (IsMoving() || _comboManager._isAttacking)
            {
                transform.rotation = Quaternion.Euler(0f, movementAngle, 0f);
                direction = _moveDirection;
            }
            
            _isDashing = true;
            _comboManager.EndCombo();
            _comboManager.SuspendAttack();
            SuspendRotation();
            SuspendMovement();

            _rigidbody.velocity = direction * (_moveSpeed + _dashSpeed);
            _rigidbody.freezeRotation = true;
            
            if (_animator != null) _animator.Play("Dash");
            
            yield return new WaitForSeconds(_dashTime);

            _comboManager.RegainAttack();
            RegainRotation();
            RegainMovement();
            _isDashing = false;
        }

        public bool IsMoving()
        {
            if (_moveInput == Vector2.zero || _isMovementSuspended || _isDashing)
            {
                return false;
            }

            return true;
        }

        public void SuspendRotation()
        {
            _isRotationSuspended = true;
            _rigidbody.freezeRotation = true;
        }

        public void SuspendMovement()
        {
            _isMovementSuspended = true;
        }
        
        public void RegainRotation()
        {
            _isRotationSuspended = false;
            _rigidbody.freezeRotation = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        public void RegainMovement()
        {
            _isMovementSuspended = false;
        }
    }
}