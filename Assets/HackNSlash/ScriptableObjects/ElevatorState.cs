using UnityEngine;

namespace HackNSlash.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ElevatorState", menuName = "ScriptableObjects/ElevatorState", order = 1)]
    public class ElevatorState : ScriptableObject
    {
        public bool isDown;
        public bool startOnElevator = false;

        public void Reset()
        {
            isDown = true;
            startOnElevator = false;
        }
    }
}