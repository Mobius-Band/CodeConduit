using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HackNSlash.Scripts.GameManagement
{
    public static class SceneReferenceExtensions
    {
        public static void SafeLoad(this SceneReference sceneReference)
        {
#if UNITY_EDITOR
            if (!sceneReference.IsSafeToUse)
            {
                Debug.Log("Scene can´t be used. Please guarantee it is included in build list " +
                          "and active");
                return;
            }
#endif
            SceneManager.LoadScene(sceneReference.BuildIndex);
        }
    }
}