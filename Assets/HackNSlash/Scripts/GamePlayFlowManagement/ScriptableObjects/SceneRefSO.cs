using System;
using Eflatun.SceneReference;
using HackNSlash.Scripts.GameManagement;
using UnityEngine;

namespace HackNSlash.Scripts.GamePlayFlowManagement
{
    [CreateAssetMenu(fileName = "Scene Refs", menuName = "PersistentData/SceneRefs", order = 1)]
    public class SceneRefSO : ScriptableObject
    {
        [Header("USER INFO SCENES")]
        [SerializeField] private SceneReference titleScreenScene;
        [SerializeField] private SceneReference mainMenuScene;
        [SerializeField] private SceneReference victoryScene;
        [SerializeField] private SceneReference gameOverScene;

        [Header("GAMEPLAY SCENES")] 
        [SerializeField] private GameplaySceneType firstGameplayType;
        [SerializeField] private SceneReference[] physicalWorldScenes;
        [SerializeField] private SceneReference[] digitalWorldScenes;

        private int currentDigitalSceneIndex = 0;
        private int currentPhysicalSceneIndex = 0;

        public void LoadTitleScreen() => titleScreenScene.SafeLoad();

        public void LoadMainMenu() => mainMenuScene.SafeLoad();

        public void LoadVictoryScene() => victoryScene.SafeLoad();

        public void LoadGameOverScene() => gameOverScene.SafeLoad();

        public void LoadFirstGameplayScene(GameplaySceneType sceneType)
        {
            switch (sceneType)
            {
                case GameplaySceneType.DigitalWorld:
                    digitalWorldScenes[currentDigitalSceneIndex = 0].SafeLoad();
                    break;
                case GameplaySceneType.PhysicalWorld:
                    physicalWorldScenes[currentPhysicalSceneIndex = 0].SafeLoad();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, null);
            }
        }

        public void LoadFirstGameplayScene()
        {
            LoadFirstGameplayScene(firstGameplayType);
        }

        public void LoadCurrentGameplayScene(GameplaySceneType sceneType)
        {
            switch (sceneType)
            {
                case GameplaySceneType.DigitalWorld:
                    digitalWorldScenes[currentDigitalSceneIndex].SafeLoad();
                    break;
                case GameplaySceneType.PhysicalWorld:
                    physicalWorldScenes[currentPhysicalSceneIndex].SafeLoad();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, null);
            }
        }
        
        public void LoadNextGameplayScene(GameplaySceneType sceneType)
        {
            switch (sceneType)
            {
                case GameplaySceneType.DigitalWorld:
                    digitalWorldScenes[++currentDigitalSceneIndex].SafeLoad();
                    break;
                case GameplaySceneType.PhysicalWorld:
                    physicalWorldScenes[++currentPhysicalSceneIndex].SafeLoad();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, null);
            }
        }
        
        public void LoadPreviousGameplayScene(GameplaySceneType sceneType)
        {
            switch (sceneType)
            {
                case GameplaySceneType.DigitalWorld:
                    digitalWorldScenes[--currentDigitalSceneIndex].SafeLoad();
                    break;
                case GameplaySceneType.PhysicalWorld:
                    physicalWorldScenes[--currentPhysicalSceneIndex].SafeLoad();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, null);
            }
        }
    }
}



