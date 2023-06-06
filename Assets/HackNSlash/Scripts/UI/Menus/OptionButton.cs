using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HackNSlash.Scripts.UI.Menus
{
    public class OptionButton : MonoBehaviour
    {
        [SerializeField] private ButtonReactor button;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image bottomLine;
        [SerializeField] private float fadeDuration;
        [SerializeField] private TMP_FontAsset selectedFont;
        [SerializeField] private int selectedFontSize;

        private TMP_FontAsset unselectedFont;
        private float unselectedFontSize;
        private Sequence fillSequence;

        private void Awake()
        {
            unselectedFont = text.font;
            unselectedFontSize = text.fontSize;
        }
        
        private void OnEnable()
        {
            ResetLabel();
            SetAnimations();
        }
        
        private void OnDisable()
        {
            ResetLabel();
        }

        public void SetAnimations()
        {
            button.OnSelected.AddListener(ExecuteSelectionAnimation);
            button.OnDeselected.AddListener(ExecuteDeselectionAnimation);
        }
        
        private void ResetLabel()
        {
            backgroundImage.fillAmount = 0;
            bottomLine.fillAmount = 1;

            text.font = unselectedFont;
            text.fontSize = unselectedFontSize;
        }

        private void Fill(bool on)
        {
            fillSequence.Kill();
            fillSequence = DOTween.Sequence();
            fillSequence.SetUpdate(UpdateType.Late, true);
            fillSequence.Append(backgroundImage.DOFillAmount(on ? 1 : 0, fadeDuration));
            fillSequence.Join(bottomLine.DOFillAmount(on ? 0 : 1, fadeDuration));
            fillSequence.Play();
        }

        private void SetFont(bool on)
        {
            text.font = on ? selectedFont : unselectedFont;
            text.fontSize = on ? selectedFontSize : unselectedFontSize;
        }

        private void ExecuteAnimation(bool isSelected)
        {
            Fill(isSelected);
            SetFont(isSelected);
        }
        private void ExecuteSelectionAnimation() => ExecuteAnimation(true);
        private void ExecuteDeselectionAnimation() => ExecuteAnimation(false);
    }
}