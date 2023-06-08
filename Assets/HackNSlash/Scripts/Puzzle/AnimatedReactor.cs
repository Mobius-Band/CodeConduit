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
        // private bool closed = false;

        private void Start()
        {
            _renderer = GetComponentInChildren<Renderer>();
            _audioManager = GetComponentInChildren<AudioManager>();
        }

        public override void React(bool isOn)
        {
            AlphaLerp(isOn);
        }

        private void AlphaLerp(bool closed)
        {
            _audioManager.Play("doorActivate");

            Debug.Log("isOpned: " + closed);
            Sequence fadeSequence = DOTween.Sequence();
            fadeSequence.Append(_renderer.materials[1].DOFade(closed ? 0 : 1 , toggleDuration/4));
            fadeSequence.Join(_renderer.materials[0].DOFloat(closed ? 0 : 1, "_AlphaController", toggleDuration / 4));
            fadeSequence.OnComplete(() => GetComponent<Collider>().enabled = !closed);
            fadeSequence.Play();
        }
        
        
        
    }
}