using UnityEngine;

namespace HackNSlash.Scripts.Util
{
    public class Permanent : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}