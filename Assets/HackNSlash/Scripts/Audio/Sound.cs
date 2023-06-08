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
        [Range(0.0f, 1.0f)]
        public float spatialBlend;
        public bool startPlaying;

        [HideInInspector] public AudioSource source;
    }
}
