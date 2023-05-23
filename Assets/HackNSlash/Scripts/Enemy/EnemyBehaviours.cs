using System;
using System.Collections;
using Combat;
using HackNSlash.Scripts.Util;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Transform = UnityEngine.Transform;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AttackManager))]
public class EnemyBehaviours : MonoBehaviour
{
    [SerializeField] public Hurtbox _hurtbox;
    [FormerlySerializedAs("target")]
    [Space]
    [HideInInspector] public Transform attackTarget;
    [SerializeField] private float _fleeingMovementMultiplier = 2;
    [SerializeField] private float _hoverDuration = 0.5f;
    [SerializeField] private float _targetDefinitionInterval = 1f;
    [SerializeField] private float _attackIntervalDuration = 5f;
    [SerializeField] private float _attackEnablingDistance = 15f;
    [SerializeField] private float _fleeingDistance = 9f;
    [SerializeField] private float _damageFreezeDuration = 2f;
    [SerializeField] private float _rotationSpeed = 90f;
    
    private NavMeshAgent _navMeshAgent;
    private AttackManager _attackManager;
    private NavMeshMovementStats _movementStats;

    private Coroutine destinationUpdater;
    private Coroutine fireRoutine;

    private Vector3? _repositionTarget;
    private bool _isTargetNull;
    private bool _isFrozen;

    public bool IsFrozen => _isFrozen;
    public float HoverDuration => _hoverDuration;
    public float AttackIntervalDuration => _attackIntervalDuration;

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _attackManager = GetComponent<AttackManager>();

        _movementStats = new NavMeshMovementStats().SavePermanentStats(_navMeshAgent);

        _hurtbox.OnHitReceived += _ => Freeze();
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
            Debug.LogError($"{name}'s attackTarget is null");
            yield return null;
        }
        while (true)
        {
            float goalDistance = (_fleeingDistance + _attackEnablingDistance) / 2;
            _repositionTarget = attackTarget.position.GetRandomXZPositionAround(goalDistance);
            _navMeshAgent.SetDestination(_repositionTarget.Value);
            if (onlyOnce)
            {
                yield break;
            }
            yield return new WaitForSeconds(_targetDefinitionInterval);
        }
    }
    
    private Vector3 TargetDirection => attackTarget.position - transform.position;
    private float AngleTowardsTarget => Vector3.Angle(transform.forward, TargetDirection);
    private IEnumerator RotateTowardsPlayerCoroutine()
    {
        while (AngleTowardsTarget > 2f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(TargetDirection);
            float rotationStep = _rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationStep);
            yield return null;
        }
    }

    public void StartTargetUpdate()
    {
        // if (IsWithinFleeingDistance())
        // {
        //     _movementStats.MultiplyAgentStats(ref _navMeshAgent, _fleeingMovementMultiplier);
        // }
        // else
        // {
        //     _movementStats.ResetAgentStats(ref _navMeshAgent);
        // }
        destinationUpdater = StartCoroutine(UpdateDestinationCoroutine(IsWithinFleeingDistance()));
    }

    public void CeaseTargetUpdate()
    {
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

    public bool CanAttack() => HasReachedAttackDistance() && !IsWithinFleeingDistance();
    
    #endregion

    #region OFFENSIVE

    public void Fire()
    {
        _repositionTarget = null;
        _attackManager.Attack(0);
    }

    private IEnumerator FireCoroutine()
    {
        while (true)
        {
            yield return StartCoroutine(RotateTowardsPlayerCoroutine());
            Fire();
            yield return new WaitForSeconds(_attackIntervalDuration);    
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

    #endregion

    private IEnumerator FreezeCoroutine()
    {
        StopAllCoroutines();
        _isFrozen = true;
        yield return new WaitForSeconds(_damageFreezeDuration);
        _isFrozen = false;
    }

    public void Freeze()
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
