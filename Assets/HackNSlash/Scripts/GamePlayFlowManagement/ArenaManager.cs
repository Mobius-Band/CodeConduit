using System;
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

        private void Awake()
        {
            connectionPopup.onBlinkStart.AddListener(() => _audioManager.Play("connected"));
            connectionPopup.onBlinkEnd.AddListener(threatPopup.Blink);
            threatPopup.onBlinkStart.AddListener(() => _audioManager.Play("threat"));
            threatPopup.onBlinkEnd.AddListener(enemyWaveManager.Initialize);
        }

        private void Start()
        {
            connectionPopup.Blink();
        }
    }
}