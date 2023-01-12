using UnityEngine;

namespace HackNSlash.Scripts.Puzzle
{
    [RequireComponent(typeof(Animator))]
    public class AnimatedReactor : PuzzleReactor
    {
        private Animator _anim;
        
        private void Start()
        {
            _anim = GetComponent<Animator>();
        }
        
        public override void React(bool isOn)
        {
            Debug.Log("Reacted: " + isOn);
            _anim.SetBool("isOn", isOn);
        }
    }
}