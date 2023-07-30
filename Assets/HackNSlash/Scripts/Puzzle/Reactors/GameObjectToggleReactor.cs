using System;
using UnityEngine;

namespace HackNSlash.Scripts.Puzzle
{
    public class GameObjectToggleReactor : PuzzleReactor
    {
        [SerializeField] private GameObject targetObject;
        [SerializeField] private bool startOn;

        private void Start()
        {
            if (targetObject == null)
            {
                return;
            }
            targetObject.SetActive(startOn);
        }

        public override void React(bool isOn)
        {
            targetObject.SetActive(isOn);
        }
    }
}