using HackNSlash.Scripts.GameManagement;
using UnityEngine;

public class OpenLaserWall : MonoBehaviour
{
    [SerializeField] private int doorIndex;
    [SerializeField] private Collider _collider;
    [SerializeField] private Renderer _renderer;

    private void Start()
    {
        switch (doorIndex)
        {
            case 2:
                if (GameManager.Instance.AccessData.canAccessPart2)
                {
                    _collider.enabled = false;
                    _renderer.enabled = false;
                }
                break;
            case 3:
                if (GameManager.Instance.AccessData.canAccessPart3)
                {
                    _collider.enabled = false;
                    _renderer.enabled = false;
                }
                break;
        }
    }
}
