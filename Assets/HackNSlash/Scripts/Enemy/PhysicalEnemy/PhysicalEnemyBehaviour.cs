using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using HackNSlash.ScriptableObjects;
using HackNSlash.Scripts.Player;
using UnityEngine;
using UnityEngine.AI;

namespace HackNSlash.Scripts.Enemy.PhysicalEnemy
{
    public class PhysicalEnemyBehaviour : MonoBehaviour
    {
        [Header("PATROL")] 
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private bool includeInitialPosition;

        [Header("MALFUNCTION")]
        [SerializeField] private int correspondentSceneIndex;
        [SerializeField] private AccessData generalAccessData;
        [ColorUsage(false, true)][SerializeField] private Color defunctColor;


        [Header("ALERT")] 
        [SerializeField] private bool useAlertBehaviour;
        [SerializeField] private Transform defensePoint;
        [SerializeField] private Transform threatPoint;
        [SerializeField] private float threatRadius;
        [SerializeField] private TweeningMaterials tweeningMaterials;
        [SerializeField] private float alertStateEffectEnterTime;
        [ColorUsage(false, true)][SerializeField] private Color alertEmission;
        
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;

        private Coroutine _alertRoutine;

        // private Color[] _defaultLightColors;
        private Quaternion _defaultRotation;
        private bool _isFocusOnDanger;
        private int _currentPatrolTargetIndex = 0;
        
        private bool IsOnAlert => _alertRoutine != null;

        private static readonly int isDefunctStateID = Animator.StringToHash("isDefunct");
        private static readonly int emissionColorID = Shader.PropertyToID("_EmissionColor");

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            TrySetPlayerAsThreat();
            tweeningMaterials.SetDefaultColors(emissionColorID);
            _defaultRotation = transform.rotation;
            
            if (IsDefunct())
            {
                PlayDefunctState();
                return;
            }
            // StartCoroutine(PatrolBehaviour());
            StartCoroutine(CheckForThreat());
        }

        private IEnumerator PatrolBehaviour()
        {
            var patrolGoals = patrolPoints.Select(p => p.position).ToList();
            if (includeInitialPosition)
            {
                patrolGoals.Add(transform.position);
            }

            int count = patrolGoals.Count;

            while (true)
            {
                _navMeshAgent.SetDestination(patrolGoals[_currentPatrolTargetIndex]);
                yield return new WaitUntil(() => _navMeshAgent.remainingDistance < 0.01f);
                _currentPatrolTargetIndex = (_currentPatrolTargetIndex + 1) % count;
            }
        }

        private bool IsDefunct()
        {
            return correspondentSceneIndex switch
            {
                2 => generalAccessData.canAccessPart2,
                3 => generalAccessData.canAccessPart3,
                4 => generalAccessData.canAccessPart4,
                _ => false
            };
        }

        private bool IsFocusPointInDanger()
        {
            return Vector3.Distance(defensePoint.position, threatPoint.position) < threatRadius;
        }

        private IEnumerator CheckForThreat()
        {
            if (!useAlertBehaviour)
            {
                yield break;
            }

            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                _isFocusOnDanger = IsFocusPointInDanger();

                switch (_isFocusOnDanger)
                {
                    case true when !IsOnAlert:
                        _alertRoutine = StartCoroutine(AlertBehaviour());
                        break;
                    case false when IsOnAlert:
                        HaltAlertBehaviour();
                        break;
                }
            }
        }

        private IEnumerator AlertBehaviour()
        {
            tweeningMaterials.DOColors(alertEmission, emissionColorID, alertStateEffectEnterTime);
            while (true)
            {
                transform.LookAt(threatPoint.position, Vector3.up);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        private void HaltAlertBehaviour()
        {
           tweeningMaterials.TweenToDefault(emissionColorID, alertStateEffectEnterTime);
            StopCoroutine(_alertRoutine);
            _alertRoutine = null;
            transform.DORotateQuaternion(_defaultRotation, alertStateEffectEnterTime);
        }

        private void TrySetPlayerAsThreat()
        {
            if (threatPoint != null)
            {
                return;
            }

            threatPoint = PlayerManager.Instance.transform;
        }

        private void PlayDefunctState()
        {
            tweeningMaterials.DOColors(alertEmission, emissionColorID, 0.01f);
            _animator.SetBool(isDefunctStateID, IsDefunct());
            float animLength = 1;
            tweeningMaterials.DOColors(defunctColor, emissionColorID, animLength);
        }

        private void TryDrawThreatGizmo()
        {
            if (!useAlertBehaviour)
            {
                return;
            }

            Color sphereColor = _isFocusOnDanger ? Color.red : Color.green;
            Gizmos.color = sphereColor;
            Gizmos.DrawWireSphere(defensePoint.position, threatRadius);
        }

        private void OnDrawGizmosSelected()
        {
            TryDrawThreatGizmo();
        }
    }
}