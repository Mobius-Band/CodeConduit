using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HackNSlash.Scripts.UI.Menus
{
    [RequireComponent(typeof(Button))]
    public class ButtonReactor : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        private Button _button;

        public Button Button => _button;

        public UnityEvent OnSelected;
        public UnityEvent OnDeselected;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnDisable()
        {
            OnSelected.RemoveAllListeners();
            OnDeselected.RemoveAllListeners();
        }

        public void OnSelect(BaseEventData eventData)
        {
            Debug.Log(name + " is selected");
            OnSelected?.Invoke();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Debug.Log(name + " is selected NO MORE");
            OnDeselected?.Invoke();
        }
        
    }
}