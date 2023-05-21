using System;
using UnityEngine;

namespace HackNSlash.Scripts.Puzzle
{
    public class PuzzleBridge : MonoBehaviour
    {
        [SerializeField] private PuzzleSwitch[] puzzleSwitches;
        [SerializeField] private PuzzleReactor puzzleReactor;

        private bool AreAllSwitchesActivated => Array.TrueForAll(puzzleSwitches, p => p.isActivated);
        public event Action<bool> OnAnySwitchToggled;
    
        private void Start()
        {
            foreach (var puzzleSwitch in puzzleSwitches)
            {
                puzzleSwitch.OnSwitchActivated += CheckSwitches;
                puzzleSwitch.OnSwitchDeactivated += CheckSwitches;
            }
            OnAnySwitchToggled += puzzleReactor.React;
        }

        private void CheckSwitches()
        {
            OnAnySwitchToggled?.Invoke(AreAllSwitchesActivated);
        }
    }
}
