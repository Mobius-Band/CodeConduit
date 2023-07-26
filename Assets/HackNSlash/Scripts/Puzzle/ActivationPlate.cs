using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        [SerializeField] private int lightMaterialIndex;
        [ColorUsage(false, true)][SerializeField] private Color activeLightColor;
        [SerializeField] private float transitionDuration;
        [SerializeField] private string colorPropertyName;

        private List<Collider> _collidersWithin;
        private Material _lightMaterial;
        private Color _inactiveLightColor;
        
        private int EmissionColorID => Shader.PropertyToID(colorPropertyName);

        private int ColliderQuantity => _collidersWithin.Count;

        private void Start()
        {
            _lightMaterial = renderer.materials[lightMaterialIndex];
            _inactiveLightColor = _lightMaterial.GetColor(EmissionColorID);
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
            Activate();
        }

        private void OnCollisionStay(Collision collisionInfo)
        {
            Collider other = collisionInfo.collider;
            OnTriggerEnter(other);
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
            isActivated = true;
            StopCoroutine(DeactivationTimer());
            base.Activate();
            _lightMaterial.DOColor(activeLightColor, EmissionColorID, transitionDuration);
        }

        private new void Deactivate()
        {
            isActivated = false;
            base.Deactivate();
            _lightMaterial.DOColor(_inactiveLightColor, EmissionColorID, transitionDuration);
        }
    }
}
