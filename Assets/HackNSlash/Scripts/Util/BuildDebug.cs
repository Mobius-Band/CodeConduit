using TMPro;
using UnityEngine;

public class BuildDebug : Singleton<BuildDebug>
{
    [SerializeField] private GameObject canvasPrefab;
    private GameObject canvasInstance;
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        canvasInstance = Instantiate(canvasPrefab, transform);
        textMesh = canvasInstance.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Log(string message)
    { 
        
    }
}