using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "TransformRegister", menuName = "ScriptableObjects/TransformRegister", order = 1)]

public class TransformRegister : ScriptableObject
{
    [SerializeField] private Transform[] registeredTransforms;

    public void RegisterTransforms(Transform[] transforms)
    {
        registeredTransforms = transforms;
    }

    public void GatherTransforms(ref Transform[] runtimeTransforms)
    {
        runtimeTransforms = registeredTransforms;
    }

    public void Reset()
    {
        registeredTransforms = Array.Empty<Transform>();
    }
}
