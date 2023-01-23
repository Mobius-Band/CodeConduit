using System;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Combat
{
    public class ComboManager : AttackManager
    {
        private PlayerMovement _playerMovement;
        private bool isReturningToIdle;
        private bool _hasNextAttack;

        private void Start()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        public void HandleAttackInput()
        {
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
            SetNextAttack();
            isReturningToIdle = false;
        }
        
        public void SetNextAttack()
        {
            if (currentAttackIndex < attacks.Length)
            {
                currentAttackIndex++;
            }
        }

        public void EndCombo()
        {
            isReturningToIdle = false;
            currentAttackIndex = 0;
            StopAttack();
            _playerMovement.RegainMovement();
            _playerMovement.RegainRotation();
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
                    break;
                case 1:
                    animator.Play("attack2");
                    break;
                case 2:
                    animator.Play("attack3");
                    break;
            }
        }
    }
}