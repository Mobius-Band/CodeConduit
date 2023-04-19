using UnityEngine;
using UnityEngine.VFX;

namespace HackNSlash.Scripts.VFX
{
    [System.Serializable]
    
    public class VFX
    {
        public GameObject visualEffect;
        public string name;
        public float delay;
        public float duration;
        public Vector3 rotation;
    }
}