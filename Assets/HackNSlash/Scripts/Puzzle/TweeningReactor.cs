using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace HackNSlash.Scripts.Puzzle
{
    public class TweeningReactor : PuzzleReactor
    {
        [Header("MOVEMENT")]
        [FormerlySerializedAs("openingOffset")] [SerializeField] private Vector3 tweeningOffset;
        [SerializeField] private float movementDurationInSecs;

        [Header("OBSTACLE CHECKING")]
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private float obstacleCheckInterval;

        private bool isDown = false;
        private Vector3 initialPosition;

        private Tween movementTween;

        private void Start()
        {
            initialPosition = transform.position;

        }

        private bool HasAnyObstacles()
        {
            Vector3 origin = transform.position;
            bool hasAnyObstacle = Physics.BoxCast(origin, transform.lossyScale / 2, tweeningOffset.normalized * CheckSignal(),
                transform.rotation, tweeningOffset.magnitude, obstacleMask, QueryTriggerInteraction.Ignore);
            return hasAnyObstacle;
        }

        private IEnumerator TryMove(bool isOn)
        {
            while (HasAnyObstacles())
            {
                yield return new WaitForSeconds(obstacleCheckInterval);
            }
            movementTween = transform.DOMove(initialPosition + tweeningOffset * Convert.ToInt32(isOn), movementDurationInSecs);
        }

        private int CheckSignal()
        {
            return isDown ? 1 : -1;
        }
        
        private void OnDrawGizmosSelected()
        {
            Vector3 origin = transform.position;
            Gizmos.color = HasAnyObstacles() ? Color.red : Color.green;
            ExtDebug.DrawBoxCastBox(
                origin, transform.lossyScale / 2, transform.rotation, tweeningOffset.normalized,
                tweeningOffset.magnitude, Gizmos.color);
            ExtDebug.DrawBoxCastBox(
                origin, transform.lossyScale / 2, transform.rotation, tweeningOffset.normalized * -1,
                tweeningOffset.magnitude, Gizmos.color);
        }
        
        public override void React(bool isOn)
        {
            isDown = isOn;
            StartCoroutine(TryMove(isOn));
        }

    }
}