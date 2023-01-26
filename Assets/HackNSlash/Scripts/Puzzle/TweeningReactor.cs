using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace HackNSlash.Scripts.Puzzle
{
    public class TweeningReactor : PuzzleReactor
    {
        [SerializeField] private Vector3 openingOffset;
        [SerializeField] private float movementDurationInSecs;
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
            RaycastHit[] obstacles = new RaycastHit[1];
            bool hasAnyObstacle = Physics.BoxCast(origin, transform.lossyScale / 2, openingOffset.normalized * CheckSignal(),
                Quaternion.identity, openingOffset.magnitude, obstacleMask, QueryTriggerInteraction.Ignore);
            return hasAnyObstacle;
        }

        private IEnumerator TryMove(bool isOn)
        {
            while (HasAnyObstacles())
            {
                yield return new WaitForSeconds(obstacleCheckInterval);
            }
            movementTween = transform.DOMove(initialPosition + openingOffset * Convert.ToInt32(isOn), movementDurationInSecs);
            yield break;
        }

        private int CheckSignal()
        {
            return isDown ? 1 : -1;
        }
        
        private void OnDrawGizmos()
        {
            Vector3 origin = transform.position;
            
            Gizmos.color = HasAnyObstacles() ? Color.red : Color.green;
            
            ExtDebug.DrawBoxCastBox(
                origin, transform.lossyScale / 2, Quaternion.identity, openingOffset.normalized,
                openingOffset.magnitude, Gizmos.color);
            ExtDebug.DrawBoxCastBox(
                origin, transform.lossyScale / 2, Quaternion.identity, openingOffset.normalized * -1,
                openingOffset.magnitude, Gizmos.color);
        }
        
        public override void React(bool isOn)
        {
            isDown = isOn;
            StartCoroutine(TryMove(isOn));
            
        }

    }
}