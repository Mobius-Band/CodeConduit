using System.Collections.Generic;
using UnityEngine;

namespace HackNSlash.ScriptableObjects
{
    public class SphereLocalRefs : ScriptableObject
    {
        public Dictionary<Transform, Vector3> SpherePositions = new();
        public Dictionary<Transform, bool> IsSphereDown;
    }
}