using System;
using System.Collections;
using DG.Tweening;
using HackNSlash.Scripts.Audio;
using HackNSlash.Scripts.Enemy;
using HackNSlash.UI.DigitalWorld_HUD.Popups.Scripts;
using UnityEngine;

namespace HackNSlash.Scripts.GamePlayFlowManagement
{
    public class ArenaManager : MonoBehaviour
    {
        [SerializeField] public ImageBlinker connectionPopup;
        [SerializeField] public ImageBlinker threatPopup;
        [SerializeField] public EnemyWaveManager enemyWaveManager;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private AestheticTweener[] materialTweeners;
        [SerializeField] private float materialTweeningInterval;
        [SerializeField] private bool startWaveAfterTweening;

        [Header("IF MATERIAL TWEENING THE SAME WAY")]
        [SerializeField] private bool useHomogeneousMaterialTweening;
        [SerializeField] private float tweenDuration;
        [ColorUsage(false, true)][SerializeField] private Color targetColor;
        
        private void Awake()
        {
            connectionPopup.onBlinkStart.AddListener(() => _audioManager.Play("connected"));
            connectionPopup.onBlinkEnd.AddListener(threatPopup.Blink);
            threatPopup.onBlinkStart.AddListener(() => _audioManager.Play("threat"));
            IEnumerator materialSequence = (ThreatActivationSequence(enemyWaveManager.Initialize));
            threatPopup.onBlinkEnd.AddListener(() => StartCoroutine(materialSequence));
            
            enemyWaveManager.OnAllWavesFinished.AddListener(() => StartCoroutine(ThreatDeactivationSequence()));
        }

        private void Start()
        {
            if (useHomogeneousMaterialTweening)
            {
                foreach (var tweener in materialTweeners)
                {
                    tweener.TrySetColorAndDuration(targetColor, tweenDuration);
                }
            }
            connectionPopup.Blink();
        }

        private IEnumerator ThreatActivationSequence(Action sequenceEndAction = null)
        {
            int length = materialTweeners.Length;
            materialTweeners[0].TweenTowardsTargetColor();
            for (int i = 1; i < length; i++)
            {
                if (startWaveAfterTweening && i == length - 1 && sequenceEndAction != null)
                {
                    materialTweeners[i].OnTweenedToTarget.AddListener(sequenceEndAction.Invoke);
                }
                yield return new WaitForSeconds(materialTweeningInterval);
                materialTweeners[i].TweenTowardsTargetColor();
            }
            if (!startWaveAfterTweening)
            {
                sequenceEndAction?.Invoke();
            }
        }

        private IEnumerator ThreatDeactivationSequence()
        {
            int length = materialTweeners.Length;
            materialTweeners[^1].TweenTowardsOriginalColor();
            for (int i = length - 1; i >= 0; i--)
            {
                yield return new WaitForSeconds(materialTweeningInterval);
                materialTweeners[i].TweenTowardsOriginalColor();
            }
        }

        [ContextMenu("Gather Material Tweeners")]
        public void GatherMaterialTweeners()
        {
            materialTweeners = FindObjectsOfType<AestheticTweener>();
        }
    }
}