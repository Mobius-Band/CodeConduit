using System;
using UnityEngine;

namespace HackNSlash.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public Sound[] sounds;

        public static AudioManager instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(gameObject);
            
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }

        public void Play(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
            }
            
            s.source.Play();
        }

        public void PlayRandom(int first, int last)
        {
            int index = UnityEngine.Random.Range(first, last);
            Sound s = sounds[index];

            if (s == null)
            {
                Debug.LogWarning("Sound with index: " + index + " not found!");
            }
        }
    }
}
