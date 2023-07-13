using System;
using System.Collections.Generic;
using System.Linq;
using Eflatun.SceneReference;
using HackNSlash.Scripts.GameManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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

        [Header("PHYSICAL-DIGITAL CORRELATION INDEXES")]
        [SerializeField] private int[] physicalPortalSceneIndexes;
        [SerializeField] private int[] digitalPortalSceneIndexes;

        private int currentDigitalSceneIndex = 0;
        private int currentPhysicalSceneIndex = 0;
        public int previousSceneIndex = -1;
        private int currentSceneIndex = -1;
        
        public UnityEvent OnGameOver;

        public void LoadTitleScreen() => titleScreenScene.SafeLoad();

        public void LoadMainMenu()
        {
            previousSceneIndex = -1;
            mainMenuScene.SafeLoad();
        }

        public void LoadVictoryScene() => victoryScene.SafeLoad();

        public void LoadGameOverScene()
        {
            OnGameOver?.Invoke();
            gameOverScene.SafeLoad();  
        } 

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

        public void DefinePreviousScene(Scene replaced)
        {
            previousSceneIndex = replaced.buildIndex;
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
        
        public bool IsOnMainMenu() => SceneManager.GetActiveScene().buildIndex == mainMenuScene.BuildIndex;

        public bool IsOnPhysicalWorld =>
            physicalWorldScenes.Any(scene => SceneManager.GetActiveScene().buildIndex == scene.BuildIndex);

        public bool IsOnDigitalWorld =>
            digitalWorldScenes.Any(scene => SceneManager.GetActiveScene().buildIndex == scene.BuildIndex);

        [ContextMenu("Set Portal Scenes Index")]
        public void SetPortalScenesIndex()
        {
            var portalPhysicalScenes = physicalWorldScenes.Where(pwS => pwS.Name.ToLower().Contains("portal"));
            var sceneReferences = portalPhysicalScenes as SceneReference[] ?? portalPhysicalScenes.ToArray();
            physicalPortalSceneIndexes = sceneReferences.Select(ppS => ppS.BuildIndex).ToArray();
            digitalPortalSceneIndexes = digitalWorldScenes.Select(ppS => ppS.BuildIndex).ToArray();
        }
    }
}



