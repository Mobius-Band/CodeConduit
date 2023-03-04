using System;
using UnityEngine;
using UnityEngine.Events;

namespace HackNSlash.Scripts.Portal
{
    public class UnityCallbackExposer : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnStart;

        private void Start()
        {
            OnStart.Invoke();
        }
    }
}