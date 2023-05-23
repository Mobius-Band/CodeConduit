using System;
using UnityEngine;

namespace Combat
{
    /// <summary>
    /// This class is used to store the data of a single meleeAttack.
    /// </summary>
    [Serializable]
    public class MeleeAttack : Attack
    {
        public int damage;
        public Vector3 hitboxPosition;
        public Vector3 hitboxSize;
        public float stepAmount;
        public float stepDuration;
    }
}