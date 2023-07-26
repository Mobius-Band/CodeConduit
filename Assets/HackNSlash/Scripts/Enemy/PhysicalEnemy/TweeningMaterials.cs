using System;
using DG.Tweening;
using UnityEngine;

namespace HackNSlash.Scripts.Enemy.PhysicalEnemy
{
    [Serializable]
    public class TweeningMaterials
    {
        public Renderer[] renderers;
        public int[] materialIndexes;

        private Color[] defaultColors;

        public void SetDefaultColors(int colorID)
        {
            int count = renderers.Length;
            defaultColors = new Color[count];
            for (int i = 0; i < count; i++)
            {
                defaultColors[i] = renderers[i].materials[materialIndexes[i]].GetColor(colorID);
            }
        }

        public void DOColors(Color targetColor, int colorID, float tweeningDuration)
        {
            int count = renderers.Length;
            for (int i = 0; i < count; i++)
            {
                renderers[i].materials[materialIndexes[i]].DOColor(targetColor, colorID, tweeningDuration).Play();
            }
        }

        public void TweenToDefault(int colorID, float tweeningDuration)
        {
            int count = renderers.Length;
            for (int i = 0; i < count; i++)
            {
                renderers[i].materials[materialIndexes[i]].DOColor(defaultColors[i], colorID, tweeningDuration).Play();
            }
        }
    }
}