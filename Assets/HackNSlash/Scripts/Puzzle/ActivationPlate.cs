using System.Collections;
using HackNSlash.Scripts.Player;
using HackNSlash.Scripts.Puzzle;
using HackNSlash.Scripts.Util;
using UnityEngine;

public class ActivationPlate : PuzzleSwitch
{
    [Header("System Setup")]
    [SerializeField] private float timer = 1f;
    [SerializeField] private LayerMask triggererMask;

    [Header("Materials")] 
    [SerializeField] private Renderer renderer;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private Material inactiveMaterial;

    private int collidersWithin;

    private void Start()
    {
        renderer.material = inactiveMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggererMask.Contains(other.gameObject.layer)) 
            return;
        if (collidersWithin > 0)
        {
            return;
        }
        collidersWithin++;
        if (other.gameObject.TryGetComponent(out PlayerPickupSphere playerPickupSphere))
        {
            if (playerPickupSphere.IsHoldingSphere)
            {
                return;
            }
        }
        isActivated = true;
        StopCoroutine(DeactivationTimer());
        Activate();

    }

    private void OnTriggerExit(Collider other)
    {
        if (!triggererMask.Contains(other.gameObject.layer)) 
            return;
        collidersWithin--;
        if (collidersWithin < 0)
            return;
        isActivated = false;
        StartCoroutine(DeactivationTimer());
    }

    private IEnumerator DeactivationTimer()
    {
        yield return new WaitForSeconds(timer);
        Deactivate();
        isActivated = false;
    }

    protected new void Activate()
    {
        print(" aaaaa ");
        base.Activate();
        renderer.material = activeMaterial;
    }

    protected new void Deactivate()
    {
        print(" bbbbb ");
        base.Deactivate();
        renderer.material = inactiveMaterial;
    }
}
