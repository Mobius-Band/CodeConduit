using UnityEngine;

namespace HackNSlash.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AccessData", menuName = "ScriptableObjects/AccessData", order = 1)]
    public class AccessData : ScriptableObject
    {
        public bool canAccessPart2;
        public bool canAccessPart3;
    }
}
