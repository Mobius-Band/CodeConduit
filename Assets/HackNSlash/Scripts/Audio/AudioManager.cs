using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HackNSlash.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public Sound[] sounds;

        public static AudioManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
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
                return;
            }

            s.source.volume = s.volume;
            if (!s.source.isPlaying)
            {
                s.source.Play();
            }
        }

        public void Mute(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            s.source.volume = 0;
        }

        public void PlayRandom(string tag)
        {
            Vector2 indexes = SearchWithTag(tag);
            int index = (int)Random.Range(indexes.x, indexes.y);
            Sound s = sounds[index];

            if (s == null)
            {
                Debug.LogWarning("Sound with index: " + index + " not found!");
                return;
            }
            
            Play(s.name);
        }

        private Vector2 SearchWithTag(string tag)
        {
            List<int> withTag = new List<int>();
            
            int i = 0;
            foreach (var sound in sounds)
            {
                if (sound.tag == tag)
                {
                    withTag.Add(i);
                }
                i++;
            }

            Vector2 indexes = new Vector2(withTag[0], withTag[withTag.Count - 1]);
            return indexes;
        }
    }
}
