using DG.Tweening;
using UnityEngine;

namespace HackNSlash.Scripts.Puzzle
{
    public class AnimatedReactor : PuzzleReactor
    {
        [SerializeField] private float toggleDuration;
        
        public override void React(bool isOn)
        {
            GetComponentInChildren<Renderer>().material.DOFade(0, toggleDuration)
                .OnComplete(() => GetComponent<Collider>().enabled = false);
        }
    }
}