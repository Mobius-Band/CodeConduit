using System;
using System.Collections;
using System.Collections.Generic;
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

    private void OnTriggerEnter(Collider other)
    {
        if (!triggererMask.Contains(other.gameObject.layer)) 
            return;
        collidersWithin++;
        isActivated = true;
        Activate();
        StopCoroutine(DeactivationTimer());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!triggererMask.Contains(other.gameObject.layer)) 
            return;
        collidersWithin--;
        if (collidersWithin > 0)
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
        base.Activate();
        renderer.material = activeMaterial;
    }

    protected new void Deactivate()
    {
        base.Deactivate();
        renderer.material = inactiveMaterial;
    }
}
