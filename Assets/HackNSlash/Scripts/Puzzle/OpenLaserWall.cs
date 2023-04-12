using DG.Tweening;
using HackNSlash.Scripts.GameManagement;
using UnityEngine;

public class OpenLaserWall : MonoBehaviour
{
    [SerializeField] private int doorIndex;
    [SerializeField] private Collider collider;
    [SerializeField] private Renderer renderer;
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
        renderer.material.DOFade(0, toggleDuration)
            .OnComplete(() => collider.enabled = false);
    }
}
