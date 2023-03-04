using System;
using HackNSlash.Scripts.Audio;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Combat
{
    public class ComboManager : AttackManager
    {
        private PlayerMovement _playerMovement;
        [SerializeField] private AudioManager _audioManager;
        private bool isReturningToIdle;
        private bool _hasNextAttack;
        private bool _isAttackSuspended;

        private void Start()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        public void HandleAttackInput()
        {
            if (_isAttackSuspended)
            {
                return;
            }
            
            if (isReturningToIdle || !_isAttacking)
            {
                if (currentAttackIndex < attacks.Length)
                {
                    ComboAttack();
                }
            }
        }
        
        private void ComboAttack()
        {
            _playerMovement.SuspendMovement();
            _playerMovement.RegainRotation();
            Attack(currentAttackIndex);
            PlayAttackAnimation();
            isReturningToIdle = false;
        }
        
        public void SetNextAttack()
        {
            if (currentAttackIndex < attacks.Length - 1)
            {
                currentAttackIndex++;
            }
        }

        public void EndCombo()
        {
            isReturningToIdle = false;
            StopAttack();
            currentAttackIndex = 0;
            if (!_playerMovement._isDashing)
            {
                _playerMovement.RegainMovement();
                _playerMovement.RegainRotation();
            }
        }

        public void SetReturningToIdle()
        {
            isReturningToIdle = true;
            _playerMovement.RegainRotation();
        }
        
        private void PlayAttackAnimation()
        {
            switch (currentAttackIndex)
            {
                case 0:
                    animator.Play("attack1");
                    _audioManager.Play("attack1");
                    break;
                case 1:
                    animator.Play("attack2");
                    _audioManager.Play("attack2");
                    break;
                case 2:
                    animator.Play("attack3");
                    _audioManager.Play("attack3");
                    break;
            }
        }

        public void SuspendAttack()
        {
            _isAttackSuspended = true;
        }

        public void RegainAttack()
        {
            _isAttackSuspended = false;
        }
    }
}