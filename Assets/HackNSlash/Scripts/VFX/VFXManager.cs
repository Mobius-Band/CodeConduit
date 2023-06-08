using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace HackNSlash.Scripts.VFX
{
    public class VFXManager : MonoBehaviour
    {
        public VFX[] visualEffects;
        private GameObject _instantiatedObject;
        
        public void PlayVFX(string name, Transform parent, Vector3 position = default, Vector3 rotation = default)
        {
            VFX vfx = Array.Find(visualEffects, vfx => vfx.name == name);
            
            if (vfx == null)
            {
                Debug.Log("Visual Effect: " + name + " not found!");
                return;
            }

            var vfxRotation = vfx.rotation;
            if (rotation != default) vfxRotation = rotation;

            var vfxPosition = vfx.position;
            if (position != default) vfxPosition = position;
            
            StartCoroutine(SpawnAndKill(vfx.visualEffect, parent, vfxPosition, vfxRotation, vfx.delay, vfx.duration));
        }

        private IEnumerator SpawnAndKill(GameObject gameObject, Transform parent, Vector3 position, Vector3 rotation, float delay, float duration)
        {
            yield return new WaitForSeconds(delay);
            
             _instantiatedObject = Instantiate(gameObject, parent);
             _instantiatedObject.transform.localScale = Vector3.one;
             _instantiatedObject.transform.localPosition = position;
             _instantiatedObject.transform.localRotation = Quaternion.Euler(rotation.x, -parent.rotation.y + rotation.y, rotation.z);
             
            yield return new WaitForSeconds(duration);

            Destroy(_instantiatedObject);
        }
    }
}