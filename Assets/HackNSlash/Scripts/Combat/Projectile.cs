using System;
using System.Collections;
using UnityEngine;

namespace Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private Hitbox hitbox;

        private float lifetime;

        public Rigidbody Rigidbody => rigidBody;
        public Hitbox Hitbox => hitbox;

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
                    yield break;
                }
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}