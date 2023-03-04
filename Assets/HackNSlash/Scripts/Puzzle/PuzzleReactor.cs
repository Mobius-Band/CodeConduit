using UnityEngine;

namespace HackNSlash.Scripts.Puzzle
{
    public abstract class PuzzleReactor : MonoBehaviour
    {
        public abstract void React(bool isOn);
    }
}