using HackNSlash.Scripts.Audio;
using HackNSlash.Scripts.VFX;
using Player;
using UnityEngine;

namespace Combat
{
    public class ComboManager : AttackManager
    {
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private VFXManager _vfxManager;
        [SerializeField] private Transform _playerWeapon;
        private PlayerMovement _playerMovement;
        private bool isReturningToIdle;
        private bool _hasNextAttack;
        private bool _isAttackSuspended;
        private bool _isLight;
        private bool _wasLight;

        private void Start()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        public void HandleAttackInput(bool isLight)
        {
            if (_isAttackSuspended)
            {
                return;
            }
            
            if (isReturningToIdle || !_isAttacking)
            {
                if (currentAttackIndex < attacks.Length)
                {
                    _isLight = isLight;
                    ComboAttack();
                }
            }
        }
        
        private void ComboAttack()
        {
            _playerMovement.SuspendMovement();
            _playerMovement.RegainRotation();
            Attack(currentAttackIndex);
            PlayAttack();
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
        
        // also plays sound and vfx
        private void PlayAttack()
        {
            switch (currentAttackIndex)
            {
                case 0:
                    if (_isLight)
                    {
                        AttackLight1();
                        _wasLight = true;
                    }
                    else
                    {
                        AttackHeavy1();
                        _wasLight = false;
                    }
                    break;
                case 1:
                    if (_isLight)
                    {
                        if (!_wasLight) break;
                        AttackLight2();
                    }
                    else
                    {
                        if (_wasLight) break;
                        AttackHeavy2();
                    }
                    break;
                case 2:
                    if (_isLight)
                    {
                        AttackLight3();
                    }
                    else
                    {
                        AttackHeavy3();
                    }
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

        private void AttackLight1()
        {
            animator.Play("AttackLight1");
            _audioManager.Play("swoosh1");
            _vfxManager.PlayVFX("slash", transform);
        }
        
        private void AttackLight2()
        {
            animator.Play("AttackLight2");
            _audioManager.Play("swoosh3");
            _vfxManager.PlayVFX("slash2", transform);
        }
        
        private void AttackLight3()
        {
            animator.Play("AttackLight3");
            _audioManager.Play("swoosh4");
            _vfxManager.PlayVFX("slash", transform);
        }
        
        private void AttackHeavy1()
        {
            animator.Play("AttackHeavy1");
            _audioManager.Play("swoosh1");
            // _vfxManager.PlayVFX("slash", transform);
        }
        
        private void AttackHeavy2()
        {
            animator.Play("AttackHeavy2");
            _audioManager.Play("swoosh3");
            // _vfxManager.PlayVFX("slash2", transform);
        }
        
        private void AttackHeavy3()
        {
            animator.Play("AttackHeavy3");
            _audioManager.Play("swoosh4");
            // _vfxManager.PlayVFX("slash", transform);
        }
    }
}