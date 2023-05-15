using System;
using System.Collections;
using Combat;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AttackManager))]
public class EnemyBehaviours : MonoBehaviour
{
    [HideInInspector] public Transform target;
    [SerializeField] private float _hoverDuration = 0.5f;
    [SerializeField] private float _targetDefinitionInterval = 1f;
    [SerializeField] private float _attackIntervalDuration = 5f;
    //[SerializeField] private float _attackEnablingDistance;
    
    // private bool _canAttack = false;
    
    private NavMeshAgent _navMeshAgent;
    private AttackManager _attackManager;

    private Coroutine destinationUpdater;

    public float HoverDuration => _hoverDuration;
    public float AttackIntervalDuration => _attackIntervalDuration;
    
    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _attackManager = GetComponent<AttackManager>();
    }

    //void Start()
    //{
    //    StartCoroutine(UpdateDestinationCoroutine());
    //}

    //private void Update()
    //{
    //_canAttack = Vector3.SqrMagnitude(target.position - transform.position) <=
    //                  Mathf.Pow(_navMeshAgent.stoppingDistance, 2);
    //    if (_canAttack)
    //    {
    //        _attackManager.Attack(0);
    //    }
    //}

    private IEnumerator UpdateDestinationCoroutine()
    {
        while (target != null)
        {
            Debug.Log("TARGET UPDATED!");
            _navMeshAgent.SetDestination(target.position);
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
        }
        StopCoroutine(destinationUpdater);
    }

    public bool HasReachedAttackDistance()
    {
        return Vector3.SqrMagnitude(target.position - transform.position) <=
               Mathf.Pow(_navMeshAgent.stoppingDistance, 2);
    }

    public void Fire()
    {
        _attackManager.Attack(0);
    }
}
