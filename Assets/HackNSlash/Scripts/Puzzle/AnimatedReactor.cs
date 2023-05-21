using System;
using DG.Tweening;
using UnityEngine;

namespace HackNSlash.Scripts.Puzzle
{
    public class AnimatedReactor : PuzzleReactor
    {
        [SerializeField] private float toggleDuration;
        private Renderer _renderer;

        private void Start()
        {
            _renderer = GetComponentInChildren<Renderer>();
        }

        public override void React(bool isOn)
        {
            for (int i = 0; i < _renderer.materials.Length - 1; i++)
            {
                _renderer.materials[i].DOFade(0, toggleDuration)
                    .OnComplete(() => GetComponent<Collider>().enabled = false);
            }
        }
    }
}