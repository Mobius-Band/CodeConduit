using UnityEngine;

namespace HackNSlash.UI.DigitalWorld_HUD.Management.Scripts
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField] private GameObject connectPopup;

        private void Awake()
        {
            connectPopup.SetActive(false);
        }

        private void Start()
        {
            connectPopup.SetActive(true);
        }
    }
}