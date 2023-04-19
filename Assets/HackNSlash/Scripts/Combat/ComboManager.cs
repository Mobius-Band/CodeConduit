using HackNSlash.Scripts.Audio;
using HackNSlash.Scripts.VFX;
using Player;
using UnityEngine;

namespace Combat
{
    public class ComboManager : AttackManager
    {
        private PlayerMovement _playerMovement;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private VFXManager _vfxManager;
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
        
        // also plays sound and vfx (should rename)
        private void PlayAttackAnimation()
        {
            switch (currentAttackIndex)
            {
                case 0:
                    animator.Play("attack1");
                    _audioManager.Play("swoosh1");
                    _vfxManager.PlayVFX("slash", transform);
                    break;
                case 1:
                    animator.Play("attack2");
                    _audioManager.Play("swoosh3");
                    _vfxManager.PlayVFX("slash2", transform);
                    break;
                case 2:
                    animator.Play("attack3");
                    _audioManager.Play("swoosh4");
                    _vfxManager.PlayVFX("slash", transform);
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