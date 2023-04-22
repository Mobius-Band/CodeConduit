using System;
using System.Collections;
using UnityEngine;

namespace HackNSlash.Scripts.VFX
{
    public class VFXManager : MonoBehaviour
    {
        public VFX[] visualEffects;
        private GameObject _instantiatedObject;
        
        public void PlayVFX(string name, Transform parent, Vector3 rotation = default)
        {
            VFX vfx = Array.Find(visualEffects, vfx => vfx.name == name);
            
            if (vfx == null)
            {
                Debug.Log("Visual Effect: " + name + " not found!");
                return;
            }

            var vfxRotation = vfx.rotation;
            if (rotation != default) vfxRotation = rotation;
            StartCoroutine(SpawnAndKill(vfx.visualEffect, parent, vfxRotation, vfx.delay, vfx.duration));
        }

        private IEnumerator SpawnAndKill(GameObject gameObject, Transform parent, Vector3 rotation, float delay, float duration)
        {
            yield return new WaitForSeconds(delay);
            
             _instantiatedObject = Instantiate(gameObject, parent);
             _instantiatedObject.transform.localScale = Vector3.one;
             _instantiatedObject.transform.localPosition = Vector3.zero;
             _instantiatedObject.transform.rotation = Quaternion.Euler(rotation);
             
            yield return new WaitForSeconds(duration);

            Destroy(_instantiatedObject);
        }
    }
}