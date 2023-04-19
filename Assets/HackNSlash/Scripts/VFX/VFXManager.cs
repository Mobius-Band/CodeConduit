using System;
using System.Collections;
using UnityEngine;

namespace HackNSlash.Scripts.VFX
{
    public class VFXManager : MonoBehaviour
    {
        public VFX[] visualEffects;
        private GameObject _instantiatedObject;
        
        public void PlayVFX(string name, Transform parent)
        {
            VFX vfx = Array.Find(visualEffects, vfx => vfx.name == name);
            
            if (vfx == null)
            {
                Debug.LogWarning("Visual Effect: " + name + " not found!");
                return;
            }

            StartCoroutine(SpawnAndKill(vfx.visualEffect, parent, vfx.rotation, vfx.delay, vfx.duration));
        }

        private IEnumerator SpawnAndKill(GameObject gameObject, Transform parent, Vector3 rotation, float delay, float duration)
        {
            yield return new WaitForSeconds(delay);
            
             _instantiatedObject = Instantiate(gameObject, parent);
             _instantiatedObject.transform.rotation = Quaternion.Euler(rotation);
             
            yield return new WaitForSeconds(duration);

            Destroy(_instantiatedObject);
        }
    }
}