using System;
using UnityEngine;

namespace Combat
{
    [Serializable]
    public class ProjectileAttack : Attack
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private GameObject speed;
        [SerializeField] private Transform origin;
        [SerializeField] private Vector3 localDirection;

        public void Execute()
        {
            UnityEngine.Object.Instantiate(projectilePrefab, origin.position, origin.rotation);
            
        }
    }
}