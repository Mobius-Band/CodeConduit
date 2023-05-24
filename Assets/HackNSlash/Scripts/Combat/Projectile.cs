using System;
using System.Collections;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Hitbox))]
    public class Projectile : MonoBehaviour
    {
        public Rigidbody Rigidbody { get; private set; }
        public Hitbox Hitbox { get; private set; }
        private float lifetime;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Hitbox = GetComponent<Hitbox>();
        }

        public void SetLastLifetime(float value)
        {
            lifetime = value;
            StartCoroutine(DestroyOnLifetimeCeased());
        }

        private IEnumerator DestroyOnLifetimeCeased()
        {
            float timer = 0;
            while (true)
            {
                if (timer > lifetime)
                {
                    Destroy(gameObject);
                }
                else
                {
                    timer += Time.deltaTime;
                }
            }
        }
    }
}