using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace HackNSlash.Scripts.Enemy.PhysicalEnemy
{
    public class PhysicalEnemyBehaviour : MonoBehaviour
    {
        [Header("PATROL")] 
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private bool includeInitialPosition;
        [SerializeField] private float patrolDuration;

        private Rigidbody _rigidbody;
        private Sequence _currentBehaviourSequence;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            InitializePatrol();
        }

        private void InitializePatrol()
        {

            var patrolGoals = patrolPoints.Select(p => p.position).ToList();
            if (includeInitialPosition)
            {
               patrolGoals.Add(transform.position);
            }
            _currentBehaviourSequence = DOTween.Sequence();
            _currentBehaviourSequence.Append(_rigidbody.d)

            // var enumerable = patrolGoals as Vector3[] ?? patrolGoals.ToArray();
            // enumerable[1]
            // _currentBehaviourSequence = _rigidbody.DOPath(enumerable.ToArray(), patrolDuration)
            //     .SetLookAt(0.5f)
            //     // .SetOptions(AxisConstraint.Y, AxisConstraint.X | AxisConstraint.Z)
            //     .SetLoops(-1, LoopType.Yoyo);
        }
    }
}