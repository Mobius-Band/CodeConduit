using System.Collections;
using DG.Tweening;
using HackNSlash.Scripts.Audio;
using UnityEngine;

namespace HackNSlash.Scripts.Puzzle
{
    public class AnimatedReactor : PuzzleReactor
    {
        [SerializeField] private float toggleDuration;
        private Renderer _renderer;
        private AudioManager _audioManager;
        private float _alphaValue;
        private bool _opened = false;

        private void Start()
        {
            _renderer = GetComponentInChildren<Renderer>();
            _audioManager = GetComponentInChildren<AudioManager>();
        }

        public override void React(bool isOn)
        {
            Debug.Log($"{name} should be on: {isOn}");
            if (isOn)
            {
                StartCoroutine(AlphaLerp());
            }
        }

        private IEnumerator AlphaLerp()
        {
            if (_opened)
            {
                yield break;
            }
            
            _audioManager.Play("doorActivate");
            
            _renderer.materials[1].DOFade(0, toggleDuration/4)
                .OnComplete(() => GetComponent<Collider>().enabled = false);
            
            for (float i = 1; i > 0; i -= 0.1f)
            {
                _renderer.materials[0].SetFloat("_AlphaController", i);
                yield return new WaitForSeconds(0.1f);
            }

            _opened = true;
            _renderer.materials[0].SetFloat("_AlphaController", 0.0f);
        }
    }
}