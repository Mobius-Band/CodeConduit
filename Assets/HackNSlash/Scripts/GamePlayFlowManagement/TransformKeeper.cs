using HackNSlash.Scripts.GameManagement;
using UnityEngine;

public class TransformKeeper : MonoBehaviour
{
    [SerializeField] private TransformRegister register;
    [SerializeField] private Transform[] targetTransforms;

    private void Start()
    {
        TrySetRuntimeTransforms();
    }

    public void DefineTransformsPersistence(int targetSceneIndex)
    {
        bool isNextSceneOnDigitalWorld = GameManager.Instance.SceneManager.IsOnDigitalWorld(targetSceneIndex);
        bool isTransitioningToDigitalWorld = 
            GameManager.Instance.SceneManager.IsCurrentlyOnPhysicalWorld() && isNextSceneOnDigitalWorld;
        if (isTransitioningToDigitalWorld)
        {
            RegisterTransforms();
        }
        else
        {
            ResetRegister();
        }
    }
    
    public void RegisterTransforms()
    {
        register.RegisterTransformsIntoDatabase(targetTransforms);
    }

    private void TrySetRuntimeTransforms()
    {
        register.TrySetRuntimeTransforms(ref targetTransforms);
    }

    public void ResetRegister()
    {
        register.Reset();
    }
}
