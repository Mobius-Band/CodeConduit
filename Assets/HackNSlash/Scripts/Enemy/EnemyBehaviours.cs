using System.Collections;
using Combat;
using HackNSlash.Scripts.Util;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AttackManager))]
public class EnemyBehaviours : MonoBehaviour
{
    [SerializeField] public Hurtbox _hurtbox;
    [Space]
    [HideInInspector] public Transform target;
    [SerializeField] private float _hoverDuration = 0.5f;
    [SerializeField] private float _targetDefinitionInterval = 1f;
    [SerializeField] private float _attackIntervalDuration = 5f;
    [SerializeField] private float _attackEnablingDistance = 15f;
    [SerializeField] private float _fleeingDistance = 9f;
    [SerializeField] private float _damageFreezeDuration = 2f;
    
    private NavMeshAgent _navMeshAgent;
    private AttackManager _attackManager;

    private Coroutine destinationUpdater;
    private Coroutine fireRoutine;
    
    private bool _isTargetNull;
    private bool _isFrozen;

    public bool IsFrozen => _isFrozen;
    public float HoverDuration => _hoverDuration;
    public float AttackIntervalDuration => _attackIntervalDuration;
    
    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _attackManager = GetComponent<AttackManager>();

        _hurtbox.OnHitReceived += _ => Freeze();
    }
    
    private void Start()
    {
        _isTargetNull = target == null;
    }

    #region POSITIONING

    private IEnumerator UpdateDestinationCoroutine()
    {
        if (_isTargetNull)
        {
            Debug.LogError($"{name}'s target is null");
            yield return null;
        }
        while (!HasReachedAttackDistance())
        {
            _navMeshAgent.SetDestination(target.position.GetRandomXZPosition(_attackEnablingDistance));
            yield return new WaitForSeconds(_targetDefinitionInterval);
        }
    }

    public void StartTargetUpdate()
    {
        destinationUpdater = StartCoroutine(UpdateDestinationCoroutine());
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
        return Vector3.SqrMagnitude(target.position - transform.position) <=
               Mathf.Pow(_attackEnablingDistance, 2);
    }

    public bool IsWithinFleeingDistance()
    {
        return Vector3.SqrMagnitude(target.position - transform.position) <=
               Mathf.Pow(_fleeingDistance, 2);
    }

    #endregion

    #region OFFENSIVE

    public void Fire()
    {
        _attackManager.Attack(0);
    }

    private IEnumerator FireCoroutine()
    {
        Fire();
        yield return new WaitForSeconds(_attackIntervalDuration);
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
}
