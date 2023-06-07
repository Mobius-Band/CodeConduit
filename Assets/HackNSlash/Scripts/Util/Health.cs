using System;
using Enemy;
using HackNSlash.Scripts.GameManagement;
using UnityEngine;

namespace Util
{
    public abstract class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int _currentHealth;
        private TakeDamageEffect _takeDamageEffect;
        private const int MinHealth = 0;
        
        public Action<int, int> OnHealthChanged;
        public Action OnDeath;
        
        public float HealthPercentage => (float) _currentHealth / maxHealth;

        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                if (value <= MinHealth)
                {
                    _currentHealth = MinHealth;
                    Die();
                }
                else if (value > maxHealth)
                {
                    _currentHealth = maxHealth;
                }
                else
                {
                    _currentHealth = value;
                }
                OnHealthChanged?.Invoke(_currentHealth, maxHealth);
            }
        }

        protected void Start()
        {
            _takeDamageEffect = GetComponent<TakeDamageEffect>();
            _currentHealth = maxHealth;
        }

        public void GainHealth(int amount)
        {
            CurrentHealth += amount;
        }
        
        public void TakeDamage(int amount)
        {
            CurrentHealth -= amount;
            StartCoroutine(_takeDamageEffect.TakeDamageEffectCoroutine());
        }

        protected abstract void Die();
    }
}
