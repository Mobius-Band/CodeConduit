using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using URPGlitch.Runtime.DigitalGlitch;

public class CameraGlitchController : MonoBehaviour
{
    [SerializeField] Volume volume;
    [SerializeField] bool updateValuesOnEveryFrame = false;

    [Header("USE ONLY IF UPDATING EVERY FRAME")] 
    [SerializeField][Range(0, 1)] private float digitalGlitchStrength; 
    
    private bool canUseGlitch;
    private DigitalGlitchVolume glitchProfile;

    public float DigitalGlitchStrength
    {
        get => glitchProfile.intensity.value;
        set => glitchProfile.intensity.value = Mathf.Clamp01(value);
    } 

    private void Start()
    {
        canUseGlitch = volume.profile.TryGet(out glitchProfile);
        if (!canUseGlitch)
        {
            Debug.Log("Glitch profile hasn't been found!");
        }
    }

    public void Update()
    {
        if (!updateValuesOnEveryFrame)
        {
            return;
        }

        DigitalGlitchStrength = digitalGlitchStrength;
    }

    public void EnableValueUpdateOnEveryFrame(bool value)
    {
        updateValuesOnEveryFrame = value;
    }
}
