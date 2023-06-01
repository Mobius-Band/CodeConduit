using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HackNSlash.Scripts.GameManagement;
using UnityEngine;

namespace HackNSlash.Scripts.Puzzle
{
    public class OpenLaserWall : MonoBehaviour
    {
        [SerializeField] private int doorIndex;
        [SerializeField] private new Collider collider;
        [SerializeField] private new Renderer renderer;
        [SerializeField] private float toggleDuration;
        private bool _opened;

        private void Start()
        {
            switch (doorIndex)
            {
                case 2:
                    if (GameManager.Instance.AccessData.canAccessPart2)
                    {
                        DisableDoor();
                    }
                    break;
                case 3:
                    if (GameManager.Instance.AccessData.canAccessPart3)
                    {
                        DisableDoor();
                    }
                    break;
            }
        }

        private void DisableDoor()
        {
            StartCoroutine(AlphaLerp());
        }
        
        private IEnumerator AlphaLerp()
        {
            if (_opened)
            {
                yield break;
            }
            
            renderer.materials[0].DOFade(0, toggleDuration/4)
                .OnComplete(() => collider.enabled = false);
            
            for (float i = 1; i > 0; i -= 0.1f)
            {
                renderer.materials[1].SetFloat("_AlphaController", i);
                yield return new WaitForSeconds(0.1f);
            }

            _opened = true;
            renderer.materials[1].SetFloat("_AlphaController", 0.0f);
        }
    }
}
