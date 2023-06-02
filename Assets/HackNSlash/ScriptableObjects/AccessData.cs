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
        [SerializeField] private SceneReference digitalPart4Scene;
        public SceneReference DigitalPart2Scene => digitalPart2Scene;
        public SceneReference DigitalPart3Scene => digitalPart3Scene;
        public SceneReference DigitalPart4Scene => digitalPart4Scene;

        [Header("PERMISSIONS")]
        public bool canAccessPart2;
        public bool canAccessPart3;
        public bool canAccessPart4;

        public void EraseAccess()
        {
            canAccessPart2 = canAccessPart3 = canAccessPart4 = false;
        }
    }
}
