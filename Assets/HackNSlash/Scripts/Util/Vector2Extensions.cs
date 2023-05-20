using UnityEngine;

namespace HackNSlash.Scripts.Util
{
    public static class Vector3Extensions
    {
        public static Vector3 GetRandomXZPositionAround(this Vector3 center, float radius)
        {
            Vector2 circle = Random.insideUnitCircle * radius;
            Vector3 newPosition = center;
            newPosition.x += circle.x;
            newPosition.z += circle.y;
            return newPosition;
        } 
    }
}