using UnityEngine;

namespace HackNSlash.Scripts.UI
{
    public class GameplayUIManager : MonoBehaviour
    {
        public void SetPlayMode(bool isPlayMode)
        {
            Cursor.visible = !isPlayMode;
            Cursor.lockState = isPlayMode ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}