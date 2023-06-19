using System;
using System.Collections;
using Player;
using UnityEngine;
// using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace HackNSlash.Scripts.PlotPanel
{
    public class ScrollInput : MonoBehaviour
    {
        private Scrollbar _scrollbar;

        private void Start()
        {
            _scrollbar.value = 0;
            // PlayerInputManager.Instance.InputActions.Player.Move.
                _movement.MoveInput = _input.InputActions.PuzzlePlayer.Move.ReadValue<Vector2>(
        }

        private IEnumerator check()
        {
            throw new NotImplementedException();
        }
    }
}