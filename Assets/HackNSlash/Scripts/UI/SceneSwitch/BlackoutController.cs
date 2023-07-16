using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

public class BlackoutController : MonoBehaviour
{
    [Header("ANIMATION SETUP")]
    [SerializeField] private float fadeDuration;
    [SerializeField] private Image blackoutPanel;
    // [SerializeField] private Animator animator;
    // [SerializeField] private string fadeInAccessor;
    // [SerializeField] private string fadeOutAccessor;
    
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
        // animator.SetTrigger(fadeInAccessor);
        fade.Kill();
        Color color = blackoutPanel.color;
        color.a = 0;
        blackoutPanel.color = color;
        fade = blackoutPanel.DOFade(1f, fadeDuration);
        fade.onPlay += OnFadeInStarted;
        fade.onComplete += OnFadeInEnded;
    }
    
    public void FadeOut()
    {
        // animator.SetTrigger(fadeOutAccessor);
        Color color = blackoutPanel.color;
        color.a = 1;
        blackoutPanel.color = color;
        Tween fade = blackoutPanel.DOFade(0f, fadeDuration);
        fade.onPlay += OnFadeOutStarted;
        fade.onComplete += OnFadeOutEnded;
    }
    
    private void OnFadeInStarted() => fadeInStarted.Raise();
    private void OnFadeInEnded() => fadeInEnded.Raise();
    private void OnFadeOutStarted() => fadeOutStarted.Raise();
    private void OnFadeOutEnded() => fadeOutEnded.Raise();
}
