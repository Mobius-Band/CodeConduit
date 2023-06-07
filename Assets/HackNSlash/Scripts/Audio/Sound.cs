using UnityEngine;

namespace HackNSlash.Scripts.Audio
{
    [System.Serializable]
    public class Sound
    {
        public AudioClip clip;
        public string name;
        public string tag;
        public float volume = 1f;
        public bool loop;
        public bool spatialize;
        public float spatialBlend;

        [HideInInspector] public AudioSource source;
    }
}
