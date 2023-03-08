using System.Collections;
using System.Collections.Generic;
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
    
    private List<Collider> collidersWithin;
    private int ColliderQuantity => collidersWithin.Count;
    // private Collider currentActivator;
    private void Start()
    {
        renderer.material = inactiveMaterial;
        collidersWithin = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (currentActivator != null)
        // {
        //     return;
        // }
        if (triggererMask.Contains(other.gameObject.layer) == false) 
            return;

        if (collidersWithin.Contains(other) == false)
        {
            collidersWithin.Add(other);
        }
        else
        {
            return;
        }
        
        // currentActivator = other;
        isActivated = true;
        StopCoroutine(DeactivationTimer());
        // Debug.Log(collidersWithin);
        Activate();

    }

    private void OnTriggerExit(Collider other)
    {
        // if (currentActivator == null)
        // {
        //     return;
        // }
        //
        // if (other != currentActivator)
        // {
        //     return;
        // }

        // currentActivator = null;
        
        if (collidersWithin.Contains(other) == true)
        {
            collidersWithin.Remove(other);
        }
        else
        {
            return;
        }
        
        if (ColliderQuantity == 0)
        {
            StartCoroutine(DeactivationTimer());
        }
    }

    private IEnumerator DeactivationTimer()
    {
        yield return new WaitForSeconds(timer);
        Deactivate();
    }

    protected new void Activate()
    {
        base.Activate();
        renderer.material = activeMaterial;
    }

    protected new void Deactivate()
    {
        isActivated = false;
        base.Deactivate();
        renderer.material = inactiveMaterial;
    }
}
