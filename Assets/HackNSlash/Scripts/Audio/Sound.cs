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
        public float pitch = 1f;
        public bool loop;

        [HideInInspector] public AudioSource source;
    }
}
