using System;
using Core.Controllers;
using Core.Models.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Windows
{
    public class TutorialWindow : MonoBehaviour, IWindow
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Image _hintImage;
        [SerializeField] private TMP_Text _hintText;

        private IUpdateController _updateController;

        public GameObject GameObject => gameObject;

        public event EventHandler<EventArgs> OnCloseRequested;

        [Inject]
        public void Construct(IUpdateController updateController)
        {
            _updateController = updateController;
        }

        public void Init(TutorialEventData tutorialEventData)
        {
            _hintImage.sprite = tutorialEventData.Image;
            _hintText.text = tutorialEventData.HintText;
        }

        private void OnEnable()
        {
            _updateController.Pause(true);

            _closeButton.onClick.AddListener(Close);
        }

        private void OnDisable()
        {
            _updateController.Unpause(true);

            _closeButton.onClick.RemoveListener(Close);
        }

        private void Close()
        {
            OnCloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}