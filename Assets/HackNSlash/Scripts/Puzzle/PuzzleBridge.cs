using System;
using HackNSlash.Scripts.Puzzle;
using UnityEngine;

public class PuzzleBridge : MonoBehaviour
{
    [SerializeField] private PuzzleSwitch[] _puzzleSwitches;
    [SerializeField] private PuzzleReactor _puzzleReactor;

    private bool areAllSwitchesActivated => Array.TrueForAll(_puzzleSwitches, p => p.isActivated);
    public event Action<bool> OnAnySwitchToggled;
    
    private void Start()
    {
        foreach (var puzzleSwitch in _puzzleSwitches)
        {
            puzzleSwitch.OnSwitchActivated += CheckSwitches;
            puzzleSwitch.OnSwitchDeactivated += CheckSwitches;
        }
        OnAnySwitchToggled += _puzzleReactor.React;
    }

    private void CheckSwitches()
    {
        OnAnySwitchToggled?.Invoke(areAllSwitchesActivated);
    }
}
