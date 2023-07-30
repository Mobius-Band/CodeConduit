using System;
using HackNSlash.Scripts.Util;
using UnityEngine;
using UnityEngine.Events;

namespace HackNSlash.Scripts.Portal
{
    [RequireComponent(typeof(Collider))]
    public class TriggerExposer : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Collider> OnTriggerEnterEvent;
        [SerializeField] private UnityEvent<Collider> OnTriggerExitEvent;
        [SerializeField] private LayerMask acceptedMask;
        
        private Collider _collider;

        private void Start()
        {
            if (TryGetComponent(out _collider))
            {
                _collider.enabled = true;
                _collider.isTrigger = true;    
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!acceptedMask.Contains(other.gameObject.layer))
            {
                return;
            } 
            OnTriggerEnterEvent?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!acceptedMask.Contains(other.gameObject.layer))
            {
                return;
            } 
            OnTriggerExitEvent?.Invoke(other);
        }
    }
}


