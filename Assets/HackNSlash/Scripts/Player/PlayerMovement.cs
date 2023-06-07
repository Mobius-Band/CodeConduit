using System.Collections;
using Combat;
using HackNSlash.Scripts.Audio;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Range(1, 100)] 
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationTime = 1f;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private Transform _cameraHolder;
        [SerializeField] private Animator _animator;
        [Range(0, 100)] 
        [SerializeField] private float _dashSpeed;
        [Range(0, 1)] 
        [SerializeField] private float _dashTime;
        [HideInInspector] public bool _isDashing;
        public Vector2 MoveInput { get => _moveInput; set => _moveInput = value; }
        private ComboManager _comboManager;
        private Rigidbody _rigidbody;
        private Vector2 _moveInput;
        private Vector3 _moveDirection;
        private float _rotationVelocity;
        private bool _isMovementSuspended;
        private bool _isRotationSuspended;
        private float _rotationAngle;
        private float movementAngle;
        private RigidbodyConstraints originalRigidbodyConstraints;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _comboManager = GetComponent<ComboManager>();
            originalRigidbodyConstraints = _rigidbody.constraints;
        }

        private void Update()
        {
            DefineMovementAngle(out movementAngle, ref _moveDirection);

            if (_moveInput == Vector2.zero || _isRotationSuspended)
            {
                return;
            }
            
            LerpRotate(movementAngle);
            
            //_animator.SetBool("isDashing", _isDashing);
        }

        private void FixedUpdate()
        {
            if (IsMoving())
            {
                _animator.SetBool("isMoving", true);
                _rigidbody.velocity = _moveDirection * _moveSpeed;
                _audioManager.Play("walk");
            }
            else
            {
                _animator.SetBool("isMoving", false);
                _audioManager.Mute("walk");
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
        
        private IEnumerator DashCoroutine(float dashSpeed)
        {
            _isDashing = true;
            _comboManager.SuspendAttack();
            _comboManager.EndCombo();
            SuspendMovement();
            SuspendRotation();

            if (_moveInput == Vector2.zero)
            {
                _moveDirection = transform.forward;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, movementAngle, 0f);
            }
            _rigidbody.velocity = _moveDirection * (dashSpeed);
            _rigidbody.freezeRotation = true;
            
            _animator.Play("DashStart");
            _audioManager.Play("dash");

            yield return new WaitForSeconds(_dashTime);

            _isDashing = false;
            RegainMovement();
            _comboManager.RegainAttack();
            RegainRotation();
        }
        
        public void Dash()
        {
            if (!_isDashing)
            {
                StartCoroutine(DashCoroutine(_dashSpeed));
            }
        }

        public IEnumerator AttackStepCoroutine(float amount, float duration)
        {
            var direction = transform.forward;
            _rigidbody.velocity = direction * (amount);
            
            yield return new WaitForSeconds(duration);
            
            if (_comboManager._isAttacking)
            {
                _rigidbody.velocity = direction * 0;
            }
        }

        public void AttackStep(Attack attack)
        {
            StartCoroutine(AttackStepCoroutine(attack.stepAmount, attack.stepDuration));
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
            _rigidbody.constraints = originalRigidbodyConstraints;
        }

        public void RegainMovement()
        {
            EndDash();
            _isMovementSuspended = false;
        }

        public void EndDash()
        {
            _isDashing = false;
        }
    }
}