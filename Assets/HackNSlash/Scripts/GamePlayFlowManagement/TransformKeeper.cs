using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformKeeper : MonoBehaviour
{
    [SerializeField] private TransformRegister register;
    [SerializeField] private Transform[] targetTransforms;

    public void RegisterTransforms()
    {
        register.RegisterTransforms(targetTransforms);
    }

    private void TrySetRuntimeTransforms()
    {
        register.GatherTransforms(ref targetTransforms);
    }

    public void ResetRegister()
    {
        register.Reset();
    }
}
