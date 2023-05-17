using UnityEngine;

namespace HackNSlash.Scripts.Util
{
    public static class Vector3Extensions
    {
        public static Vector3 GetRandomXZPosition(this Vector3 center, float radius)
        {
            Vector2 bidimensionalPosition = Random.insideUnitCircle * radius;
            Vector3 newPosition = center;
            newPosition.x = bidimensionalPosition.x;
            newPosition.z = bidimensionalPosition.y;
            return newPosition;
        } 
    }
}