using System.Collections;
using System.Collections.Generic;
using HackNSlash.Scripts.Util;
using UnityEngine;

namespace HackNSlash.Scripts.Puzzle
{
    public class ActivationPlate : PuzzleSwitch
    {
        [Header("System Setup")]
        [SerializeField] private float timer = 1f;
        [SerializeField] private LayerMask triggerMask;

        [Header("Materials")] 
        [SerializeField] private new Renderer renderer;
        [SerializeField] private Material activeMaterial;
        [SerializeField] private Material inactiveMaterial;
    
        private List<Collider> _collidersWithin;
        private int ColliderQuantity => _collidersWithin.Count;
        
        private void Start()
        {
            renderer.material = inactiveMaterial;
            _collidersWithin = new List<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!triggerMask.Contains(other.gameObject.layer)) 
                return;

            if (!_collidersWithin.Contains(other))
            {
                _collidersWithin.Add(other);
            }
            else
            {
                return;
            }
        
            isActivated = true;
            StopCoroutine(DeactivationTimer());
            Activate();
        }

        private void OnTriggerExit(Collider other)
        {
            if (_collidersWithin.Contains(other))
            {
                _collidersWithin.Remove(other);
            }
            else
            {
                return;
            }
        
            if (ColliderQuantity == 0)
            {
                StartCoroutine(DeactivationTimer());
            }
        }

        private IEnumerator DeactivationTimer()
        {
            yield return new WaitForSeconds(timer);
            Deactivate();
        }

        private new void Activate()
        {
            base.Activate();
            renderer.material = activeMaterial;
        }

        private new void Deactivate()
        {
            isActivated = false;
            base.Deactivate();
            renderer.material = inactiveMaterial;
        }
    }
}
