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
        [SerializeField] private MaterialTweener[] materialTweeners;
        [SerializeField] private float materialTweeningInterval;
        [SerializeField] private bool startWaveAfterTweening;
        
        private void Awake()
        {
            connectionPopup.onBlinkStart.AddListener(() => _audioManager.Play("connected"));
            connectionPopup.onBlinkEnd.AddListener(threatPopup.Blink);
            threatPopup.onBlinkStart.AddListener(() => _audioManager.Play("threat"));
            IEnumerator materialSequence = (MaterialTweeningSequence(enemyWaveManager.Initialize));
            threatPopup.onBlinkEnd.AddListener(() => StartCoroutine(materialSequence));
        }

        private void Start()
        {
            connectionPopup.Blink();
        }

        private IEnumerator MaterialTweeningSequence(Action sequenceEndAction)
        {
            materialTweeners[0].TweenTowardsTargetColor();
            for (int i = 1; i < materialTweeners.Length; i++)
            {
                if (startWaveAfterTweening && i == materialTweeners.Length - 1)
                {
                    materialTweeners[i].OnTweenedToTarget.AddListener(sequenceEndAction.Invoke);
                }
                yield return new WaitForSeconds(materialTweeningInterval);
                materialTweeners[i].TweenTowardsTargetColor();
 
            }

            if (!startWaveAfterTweening)
            {
                sequenceEndAction.Invoke();
            }
        }

        [ContextMenu("Gather Material Tweeners")]
        public void GatherMaterialTweeners()
        {
            materialTweeners = FindObjectsOfType<MaterialTweener>();
        }
    }
}