using System;
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

        private void Awake()
        {
            connectionPopup.onBlinkEnd.AddListener(threatPopup.Blink);
            threatPopup.onBlinkEnd.AddListener(enemyWaveManager.Initialize);
        }

        private void Start()
        {
            connectionPopup.Blink();
        }
    }
}