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
        [SerializeField] private float scrollRate = 0.1f;
        private Scrollbar _scrollbar;
        private Coroutine _inputRoutine;
        private bool _canGetInput = false;

        // private void Start()
        // {
        //     _canGetInput = true;
        // }

        private void OnEnable()
        {
            if (!_canGetInput)
            {
                return;
            }
            _scrollbar = GetComponent<Scrollbar>();
            _scrollbar.value = 0.999f;
            _inputRoutine = StartCoroutine(CheckForScrollInput());
        }

        private void OnDisable()
        {
            if (_inputRoutine != null)
            {
                StopCoroutine(_inputRoutine);
            }
            else
            {
                _canGetInput = true;
            }
        }

        private IEnumerator CheckForScrollInput()
        {
            while (true)
            {
                float inputValue = PlayerInputManager.Instance.InputActions.Player.Move.ReadValue<Vector2>().y;
                float previousScrollBarValue = _scrollbar.value;
                float finalValue = Mathf.Clamp01(previousScrollBarValue - (inputValue * scrollRate));
                _scrollbar.value = finalValue; 
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }
    }
}