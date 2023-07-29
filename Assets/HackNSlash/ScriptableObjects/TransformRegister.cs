using System;
using HackNSlash.Scripts.GamePlayFlowManagement;
using UnityEngine;

[CreateAssetMenu(fileName = "TransformRegister", menuName = "ScriptableObjects/TransformRegister", order = 1)]

public class TransformRegister : ScriptableObject
{
    [SerializeField] private TransformData[] registeredTransforms;

    public void RegisterTransformsIntoDatabase(Transform[] transforms)
    {
        int count = transforms.Length;
        registeredTransforms = new TransformData[count];
        for (int i = 0; i < count; i++)
        {
            registeredTransforms[i] = transforms[i];
        }
    }

    public bool TrySetRuntimeTransforms(ref Transform[] runtimeTransforms)
    {
        if (registeredTransforms.Length != runtimeTransforms.Length)
        {
            return false;
        }
        
        int count = registeredTransforms.Length;
        for (int i = 0; i < count; i++)
        {
            runtimeTransforms[i].position = registeredTransforms[i].position;
            runtimeTransforms[i].rotation = registeredTransforms[i].rotation;
            runtimeTransforms[i].localScale = registeredTransforms[i].scale;
        }
        return true;
    }

    public void Reset()
    {
        Debug.Log("Resetting");

        registeredTransforms = Array.Empty<TransformData>();
    }
}
