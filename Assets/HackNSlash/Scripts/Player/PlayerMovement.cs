using System.Collections;
using Combat;
using HackNSlash.Scripts.Audio;
using UnityEngine;

namespace HackNSlash.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private Transform cameraHolder;
        [SerializeField] private Animator animator;
        [Range(1, 100)] 
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationTime = 1f;
        [Range(0, 100)] 
        [SerializeField] private float dashSpeed;
        [Range(0, 1)] 
        [SerializeField] private float dashTime;
        [HideInInspector] public bool isDashing;
        public Vector2 MoveInput { get => _moveInput; set => _moveInput = value; }
        private ComboManager _comboManager;
        private Rigidbody _rigidbody;
        private Vector2 _moveInput;
        private Vector3 _moveDirection;
        private RigidbodyConstraints _originalRigidbodyConstraints;
        private float _rotationVelocity;
        private bool _isMovementSuspended;
        private bool _isRotationSuspended;
        private float _rotationAngle;
        private float _movementAngle;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _comboManager = GetComponent<ComboManager>();
            _originalRigidbodyConstraints = _rigidbody.constraints;
        }

        private void Update()
        {
            if (IsMoving())
            {
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
            
            DefineMovementAngle(out _movementAngle, ref _moveDirection);

            if (_moveInput == Vector2.zero || _isRotationSuspended)
            {
                return;
            }
            
            LerpRotate(_movementAngle);
        }

        private void FixedUpdate()
        {
            if (IsMoving())
            {
                _rigidbody.velocity = _moveDirection * moveSpeed;
            }
        }
        
        private void LerpRotate(float movementAngle)
        {
            _rotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, movementAngle, ref _rotationVelocity,
                rotationTime / 100);
            transform.rotation = Quaternion.Euler(0f, _rotationAngle, 0f);
        }

        private void DefineMovementAngle(out float movementAngle, ref Vector3 moveDirection)
        {
            movementAngle = Mathf.Atan2(_moveInput.x, _moveInput.y) * Mathf.Rad2Deg + cameraHolder.eulerAngles.y;

            moveDirection = Quaternion.Euler(0f, movementAngle, 0f) * Vector3.forward;
            moveDirection.Normalize();
        }
        
        private IEnumerator DashCoroutine(float dashSpeed)
        {
            isDashing = true;
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
                transform.rotation = Quaternion.Euler(0f, _movementAngle, 0f);
            }
            _rigidbody.velocity = _moveDirection * (dashSpeed);
            _rigidbody.freezeRotation = true;
            
            animator.Play("DashStart");
            audioManager.Play("dash");

            yield return new WaitForSeconds(dashTime);

            isDashing = false;
            RegainMovement();
            _comboManager.RegainAttack();
            RegainRotation();
        }
        
        public void Dash()
        {
            if (!isDashing)
            {
                StartCoroutine(DashCoroutine(dashSpeed));
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

        private bool IsMoving()
        {
            if (_moveInput == Vector2.zero || _isMovementSuspended || isDashing)
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
            _rigidbody.constraints = _originalRigidbodyConstraints;
        }

        public void RegainMovement()
        {
            EndDash();
            _isMovementSuspended = false;
        }

        public void EndDash()
        {
            isDashing = false;
        }

        public void PlayStepSound()
        {
            print("step");
            audioManager.PlayRandom("walk");
        }
    }
}