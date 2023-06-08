using DG.Tweening;
using HackNSlash.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace HackNSlash.Scripts.UI
{
    public class HealthBar : MonoBehaviour
    {
        [Header("REFS")]
        [SerializeField] private Health health;
        [SerializeField] private Image fillableImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image auxiliarImage;
        [Header("MOTION")]
        [SerializeField] private float effectDuration;
        [SerializeField] private int blinkRepetitions;
        [SerializeField] private float scaleRate;
        [SerializeField] private float auxiliarThreshold;
        

        private void OnEnable()
        {
            health.OnHealthChanged += UpdateHealthBar;
        }
        
        private void OnDisable()
        {
            health.OnHealthChanged -= UpdateHealthBar;
        }
        
        private void UpdateHealthBar(int currentHealth, int maxHealth)
        {
            var healthPercentage = (float) currentHealth / maxHealth;
            fillableImage.DOFillAmount(healthPercentage, effectDuration);
            PlayBlinkAnimation(ref fillableImage, Color.red, blinkRepetitions);
            PlayScaleSequence(Vector3.one * scaleRate, ref fillableImage);
            if (backgroundImage != null)
            {
                PlayBlinkAnimation(ref backgroundImage, Color.red, blinkRepetitions);
                PlayScaleSequence(Vector3.one * scaleRate, ref backgroundImage);
            }

            if (healthPercentage < auxiliarThreshold)
            {
                auxiliarImage.DOFillAmount(healthPercentage * 2, effectDuration);
                PlayBlinkAnimation(ref auxiliarImage, Color.red, blinkRepetitions);
                PlayScaleSequence(Vector3.one * scaleRate, ref auxiliarImage);
            }
        }

        private void PlayBlinkAnimation(ref Image image, Color targetColor = default, int repetitions = 2)
        {
            Color originalColor = Color.white;
            Sequence fadeSequence = DOTween.Sequence();
            for (int i = 0; i < repetitions; i++)
            {
                fadeSequence.Append(image.DOColor(targetColor, effectDuration / repetitions / 2));
                fadeSequence.Append(image.DOColor(originalColor, effectDuration / repetitions / 2));
                // fadeSequence.Append(image.DOColor(originalColor, 0.01f));
            }
            fadeSequence.Play();
        }

        private void PlayScaleSequence(Vector3 targetScale, ref Image image)
        {
            Sequence mainScale = DOTween.Sequence();
            mainScale.Append(fillableImage.rectTransform.DOScale(targetScale, effectDuration / 2));
            mainScale.Append(fillableImage.rectTransform.DOScale(Vector3.one, effectDuration / 2));
            mainScale.Play();
        }
    }
}