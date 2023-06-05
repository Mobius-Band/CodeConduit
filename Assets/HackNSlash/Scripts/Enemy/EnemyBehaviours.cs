using System.Collections;
using Combat;
using HackNSlash.Scripts.Enemy;
using HackNSlash.Scripts.Util;
using HackNSlash.Scripts.VFX;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Transform = UnityEngine.Transform;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviours : MonoBehaviour
{
    [Header("LONG-DISTANCE ATTACK")]
    [SerializeField] private float _attackIntervalDuration_MIN;
    [SerializeField] private float _attackIntervalDuration_MAX;
    [SerializeField] private float _attackEnablingDistance = 15f;
    [SerializeField] private float _attackEnablingAngle = 5f;
    [Header("FINAL ATTACK")]
    [SerializeField] [Range(0, 100)] private float _healthThreshold;
    [Header("DEFAULT MOVEMENT")]
    [SerializeField] private float _initialHoverDuration = 0.5f;
    [SerializeField] private float _fleeingMovementMultiplier = 2;
    [SerializeField] private float _destinationDefinitionInterval = 1f;
    [SerializeField] private float _fleeingDistance = 9f;
    [SerializeField] private float _damageFreezeDuration = 2f;
    [SerializeField] private float _rotationSpeed = 90f;
    [Header("EXTERNAL REFS")]
    [SerializeField] public Hurtbox _hurtbox;
    [SerializeField] private VFXManager _vfxManager;
    [SerializeField] private EnemyAttackManager _attackManager;
    [SerializeField] private EnemyHealth _health;
    [SerializeField] private Animator _animator;
    [SerializeField] private EnemyAnimationManager _animationManager;
    [SerializeField] private string _meleeAnimationProperty;
    [HideInInspector] public Transform attackTarget;
    
    private NavMeshAgent _navMeshAgent;
    private NavMeshMovementStats _movementStats;

    private Coroutine destinationUpdater;
    private Coroutine fireRoutine;

    private Vector3? _repositionTarget;
    private float _attackIntervalDuration;
    private bool _isTargetNull;
    private bool _isFrozen;

    public bool IsFrozen => _isFrozen;
    public float InitialHoverDuration => _initialHoverDuration;
    public float AttackIntervalDuration => _attackIntervalDuration;

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _movementStats = new NavMeshMovementStats().SavePermanentStats(_navMeshAgent);
        _hurtbox.OnHitReceived += _ => TempFreeze();
        _health.OnHealthChanged += (_,_) => TryEnterDesperateMode();
        if (_vfxManager != null)
        {
            _hurtbox.OnHitReceived += _ => _vfxManager.PlayVFX("impact", transform);
        }
        _animationManager.OnFinalTransformationEnd += Unfreeze;
    }
    
    private void Start()
    {
        _isTargetNull = attackTarget == null;
    }

    #region POSITIONING

    private IEnumerator UpdateDestinationCoroutine(bool onlyOnce = false)
    {
        if (_isTargetNull)
        {
            yield return null;
        }
        while (true)
        {
            float goalDistance = (_fleeingDistance + _attackEnablingDistance) / 2;
            _repositionTarget = attackTarget.position.GetRandomXZPositionAround(goalDistance);

            _navMeshAgent.SetDestination(_repositionTarget.Value);
            if (onlyOnce)
            {
                yield return new WaitUntil(IsWithinFleeingDistance);
                yield return new WaitForSeconds(_destinationDefinitionInterval * 2f);
            }
            else
            {
                yield return new WaitForSeconds(_destinationDefinitionInterval);
            }
        }
    }
    
    private Vector3 TargetDirection => attackTarget.position - transform.position;
    private float AngleTowardsTarget => Vector3.Angle(transform.forward, TargetDirection);
    private IEnumerator RotateTowardsPlayerCoroutine()
    {
        while (AngleTowardsTarget > _attackEnablingAngle)
        {
            Quaternion targetRotation = Quaternion.LookRotation(TargetDirection);
            float rotationStep = _rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationStep);
            yield return null;
        }
    }

    public void StartTargetUpdate()
    {
        if (IsWithinFleeingDistance())
        {
            _movementStats.MultiplyAgentStats(ref _navMeshAgent, _fleeingMovementMultiplier);
        }
        else
        {
            _movementStats.ResetAgentStats(ref _navMeshAgent);
        }
        _navMeshAgent.isStopped = false;
        destinationUpdater = StartCoroutine(UpdateDestinationCoroutine(IsWithinFleeingDistance()));
    }

    public void CeaseTargetUpdate()
    {
        if (CanAttack())
        {
            _navMeshAgent.isStopped = true;
        }
        if (destinationUpdater == null)
        {
            Debug.LogWarning("Can't cease what hasn't been started");
            return;
        }
        StopCoroutine(destinationUpdater);
    }

    public bool HasReachedAttackDistance()
    { 
        return Vector3.SqrMagnitude(attackTarget.position - transform.position) <=
               Mathf.Pow(_attackEnablingDistance, 2);
    }

    public bool IsWithinFleeingDistance()
    {
        return Vector3.SqrMagnitude(attackTarget.position - transform.position) <=
               Mathf.Pow(_fleeingDistance, 2);
    }

    public bool CanAttack()
    {
      return 
          (HasReachedAttackDistance() && !IsWithinFleeingDistance())
          || IsAtLastHealth();  
    } 
    
    #endregion

    #region OFFENSIVE

    public void DefineAttackInterval()
    {
        _attackIntervalDuration = Random.Range(_attackIntervalDuration_MIN, _attackIntervalDuration_MAX);
    }
    
    public void Fire()
    {
        _repositionTarget = null;
        if (IsAtLastHealth())
        {
            _navMeshAgent.isStopped = true;
            _attackManager.Dash();
            return;
        }
        _animator.SetTrigger(_meleeAnimationProperty);
    }

    private IEnumerator FireCoroutine()
    {
        while (true)
        {
            yield return StartCoroutine(RotateTowardsPlayerCoroutine());
            DefineAttackInterval();
            yield return new WaitForSeconds(
                IsAtLastHealth() ?
                _attackIntervalDuration
                : _attackManager.DashPreparationTime);
            if (!_isFrozen)
            {
                Fire();
            }
        }
    }

    public void InitiateFireSequence()
    {
        fireRoutine = StartCoroutine(FireCoroutine());
    }

    public void StopFireSequence()
    {
        StopCoroutine(fireRoutine);
    }

    private void TryEnterDesperateMode()
    {
        if (IsAtLastHealth())
        {
            Freeze();
            _animator.Play("FinalForm");
            _animator.SetBool("isFinalForm", true);
        }
    }
    
    public bool IsAtLastHealth()
    {
        return _health.HealthPercentage * 100 < _healthThreshold;
    }

    #endregion

    private IEnumerator FreezeCoroutine()
    {
        Freeze();
        yield return new WaitForSeconds(_damageFreezeDuration);
        Unfreeze();
    }

    private void ToggleFreeze(bool value)
    {
        if (_navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.isStopped = value;
        }
        _isFrozen = value;
    }

    private void Freeze()
    {
        CeaseTargetUpdate();
        _navMeshAgent.SetDestination(transform.position);
        ToggleFreeze(true);
    } 
    private void Unfreeze() => ToggleFreeze(false);

    public void TempFreeze()
    {
        StartCoroutine(FreezeCoroutine());
    }

    private void OnDrawGizmos()
    {
        if (_repositionTarget.HasValue)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_repositionTarget.Value, 1f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackEnablingDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _fleeingDistance);
    }

    private struct NavMeshMovementStats
    {
        public float speed;
        public float angularSpeed;
        public float acceleration;

        public NavMeshMovementStats SavePermanentStats(NavMeshAgent agent)
        {
            speed = agent.speed;
            angularSpeed = agent.angularSpeed;
            acceleration = agent.acceleration;
            return this;
        }

        public void ResetAgentStats(ref NavMeshAgent agent)
        {
            agent.speed = speed;
            agent.angularSpeed = angularSpeed;
            agent.acceleration = acceleration;
        }

        public void MultiplyAgentStats(ref NavMeshAgent agent, float multiplier)
        {
            agent.speed = speed * multiplier;
            agent.angularSpeed = angularSpeed * multiplier;
            agent.acceleration = acceleration * multiplier;
        }
    }
}
