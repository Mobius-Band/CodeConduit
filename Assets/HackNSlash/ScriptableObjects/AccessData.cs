using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.Serialization;

namespace HackNSlash.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AccessData", menuName = "ScriptableObjects/AccessData", order = 1)]
    public class AccessData : ScriptableObject
    {
        [FormerlySerializedAs("laserWallScene1")]
        [Header("AREA")]
        [SerializeField] private SceneReference digitalPart2Scene;
        [SerializeField] private SceneReference digitalPart3Scene;
        public SceneReference DigitalPart2Scene => digitalPart2Scene;
        public SceneReference DigitalPart3Scene => digitalPart3Scene;

        [Header("PERMISSIONS")]
        public bool canAccessPart2;
        public bool canAccessPart3;

        public void EraseAccess()
        {
            canAccessPart2 = canAccessPart3 = false;
        }
    }
}
