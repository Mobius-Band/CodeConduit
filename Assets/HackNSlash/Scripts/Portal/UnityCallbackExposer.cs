using System;
using UnityEngine;
using UnityEngine.Events;

namespace HackNSlash.Scripts.Portal
{
    public class UnityCallbackExposer : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnStart;
        [SerializeField] private UnityEvent<Collider> OnTriggerEnterEvent;

        private void Start()
        {
            OnStart.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent.Invoke(other);
        }
    }
}