using System;
using System.Collections;
using System.Collections.Generic;
using Ez;
using HackNSlash.Scripts.Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _attackRange = 0.5f;
        [SerializeField] private int _damage = 5;
        public bool _isAttacking = false;
        private PlayerMovement _playerMovement;
        private bool _isComboing = false;
        private bool _isAnimationOver = false;
        private bool _suspendAttack;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            _animator.SetBool("isComboing", _isComboing);
        }

        public void Attack()
        {
            if (!_isAttacking && !_suspendAttack)
            {
                _isAttacking = true;
                Invoke("StopAttacking", 0.5f);
                
                if (!_isComboing)
                {
                    StartCoroutine(Combo());
                }
            }
        }

        private IEnumerator Combo()
        {
            _isComboing = true;
            _playerMovement.SuspendMovement();

            _isAnimationOver = false;
            _animator.SetTrigger("goToNextAnimation");

            while (!_isAnimationOver)
            {
                yield return null;
            }

            if (!_isAttacking)
            {
                EndCombo();
                yield break;
            }

            _isAnimationOver = false;
            _animator.SetTrigger("goToNextAnimation");
            
            while (!_isAnimationOver)
            {
                yield return null;
            }
            
            if (!_isAttacking)
            {
                EndCombo();
                yield break;
            }
            
            _isAnimationOver = false;
            _animator.SetTrigger("goToNextAnimation");

            while (!_isAnimationOver)
            {
                yield return null;
            }

            _isAnimationOver = false;
            EndCombo();
        }

        public void Hit()
        {
            Collider[] hitEnemies = Physics.OverlapSphere(_attackPoint.position, _attackRange, _enemyLayer);

            foreach (Collider enemy in hitEnemies)
            {
                enemy.gameObject.Send<IDamageable>(x => x.TakeDamage(_damage));
            }
        }

        private void StopAttacking()
        {
            _isAttacking = false;
        }

        public void SuspendAttack()
        {
            _suspendAttack = true;
        }

        public void RegainAttack()
        {
            _suspendAttack = false;
        }

        public void EndCombo()
        {
            StopAttacking();
            _isComboing = false;
            _playerMovement.RegainMovement();
            _playerMovement.RegainRotation();
        }

        public void EndAnimation()
        {
            _isAnimationOver = true;
        }
        
        private void OnDrawGizmosSelected()
        {
            if (_attackPoint == null)
                return;
            Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
        }
    }
}