using System;
using System.Collections;
using Enemy;
using UnityEngine;
using UnityEngine.Serialization;

namespace HackNSlash.Scripts.Util
{
    public abstract class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int currentHealth;
        private TakeDamageEffect _takeDamageEffect;
        private const int MinHealth = 0;
        
        public Action<int, int> OnHealthChanged;
        public Action OnDeath;
        
        public float HealthPercentage => (float) currentHealth / maxHealth;

        public int CurrentHealth
        {
            get => currentHealth;
            set
            {
                if (value <= MinHealth)
                {
                    currentHealth = MinHealth;
                    StartCoroutine(Die());
                }
                else if (value > maxHealth)
                {
                    currentHealth = maxHealth;
                }
                else
                {
                    currentHealth = value;
                }
                OnHealthChanged?.Invoke(currentHealth, maxHealth);
            }
        }

        protected void Awake()
        {
            _takeDamageEffect = GetComponent<TakeDamageEffect>();
            currentHealth = maxHealth;
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

        protected abstract IEnumerator Die();
    }
}