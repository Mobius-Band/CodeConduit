using System;
using DG.Tweening;
using UnityEngine;

namespace HackNSlash.Scripts.Puzzle
{
    public class TweeningReactor : PuzzleReactor
    {
        [SerializeField] private Vector3 openingOffset;
        [SerializeField] private float movementDurationInSecs;
        
        private Vector3 initialPosition;
        
        public override void React(bool isOn)
        {
            transform.DOMove(transform.position + openingOffset * Convert.ToInt32(isOn), movementDurationInSecs);
        }
    }
}