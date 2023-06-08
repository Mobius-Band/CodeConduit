using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HackNSlash.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public Sound[] sounds;
        private int _lastRandomIndex;

        private void Awake()
        {
            foreach (Sound sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.loop = sound.loop;
                sound.source.spatialize = sound.spatialize;
                sound.source.spatialBlend = sound.spatialBlend;

                if (!sound.spatialize) sound.spatialBlend = 0;
                
                if (sound.startPlaying) Play(sound.name);
            }
        }

        public void Play(string soundName)
        {
            Sound s = Array.Find(sounds, sound => sound.name == soundName);
            
            if (s == null)
            {
                Debug.LogWarning("Sound: " + soundName + " not found!");
                return;
            }

            s.source.volume = s.volume;
            if (!s.source.isPlaying)
            {
                s.source.Play();
            }
        }

        public void Mute(string soundName)
        {
            Sound s = Array.Find(sounds, sound => sound.name == soundName);
            
            if (s == null)
            {
                Debug.Log("Sound: " + soundName + " not found!");
                return;
            }

            s.source.volume = 0;
        }

        public void PlayRandom(string soundTag, bool loops = false)
        {
            Vector2 indexes = SearchWithTag(soundTag);
            int index = (int)Random.Range(indexes.x, indexes.y);

            if (!loops)
            {
                while (index == _lastRandomIndex)
                {
                    index = (int)Random.Range(indexes.x, indexes.y);
                }
            }
            
            _lastRandomIndex = index;
            
            Sound s = sounds[index];

            if (s == null)
            {
                Debug.Log("Sound with index: " + index + " not found!");
                return;
            }
            
            Play(s.name);
        }

        private Vector2 SearchWithTag(string soundTag)
        {
            List<int> withTag = new List<int>();
            
            int i = 0;
            foreach (var sound in sounds)
            {
                if (sound.tag == soundTag)
                {
                    withTag.Add(i);
                }
                i++;
            }

            // gets the first and last indexes that have the sound tag (they need to be adjacent on the array)
            var indexes = new Vector2(withTag[0], withTag[^1]);
            return indexes;
        }
    }
}
