using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

public class BlackoutController : MonoBehaviour
{
    [Header("ANIMATION SETUP")]
    [SerializeField] private Image blackoutPanel;
    [SerializeField] private float fadeInDuration;
    [SerializeField] private float fadeOutDuration;

    [Header("GAME EVENTS")] 
    [SerializeField] private GameEvent fadeInStarted;
    [SerializeField] private GameEvent fadeInEnded;
    [SerializeField] private GameEvent fadeOutStarted;
    [SerializeField] private GameEvent fadeOutEnded;

    private Tween fade;

    private void Start()
    {
        FadeOut();
    }

    public void FadeIn()
    {
        fade.Kill();
        Color color = blackoutPanel.color;
        color.a = 0;
        blackoutPanel.color = color;
        fade = blackoutPanel.DOFade(1f, fadeInDuration);
        fade.onPlay += OnFadeInStarted;
        fade.onComplete += OnFadeInEnded;
    }
    
    public void FadeOut()
    {
        Color color = blackoutPanel.color;
        color.a = 1;
        blackoutPanel.color = color;
        Tween fade = blackoutPanel.DOFade(0f, fadeOutDuration);
        fade.onPlay += OnFadeOutStarted;
        fade.onComplete += OnFadeOutEnded;
    }
    
    private void OnFadeInStarted() => fadeInStarted.Raise();
    private void OnFadeInEnded() => fadeInEnded.Raise();
    private void OnFadeOutStarted() => fadeOutStarted.Raise();
    private void OnFadeOutEnded() => fadeOutEnded.Raise();
}
