using DG.Tweening;
using HackNSlash.Scripts.GameManagement;
using UnityEngine;

namespace HackNSlash.Scripts.Puzzle
{
    public class OpenLaserWall : MonoBehaviour
    {
        [SerializeField] private int doorIndex;
        [SerializeField] private new Collider collider;
        [SerializeField] private new Renderer renderer;
        [SerializeField] private float toggleDuration;

        private void Start()
        {
            switch (doorIndex)
            {
                case 2:
                    if (GameManager.Instance.AccessData.canAccessPart2)
                    {
                        DisableDoor();
                    }
                    break;
                case 3:
                    if (GameManager.Instance.AccessData.canAccessPart3)
                    {
                        DisableDoor();
                    }
                    break;
            }
        }

        private void DisableDoor()
        {
            for (int i = 0; i < renderer.materials.Length - 1; i++)
            {
                renderer.materials[i].DOFade(0, toggleDuration)
                    .OnComplete(() => collider.enabled = false);
            }
        }
    }
}
