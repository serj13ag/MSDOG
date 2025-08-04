using System;
using Core.Controllers;
using Core.Models.Data;
using Gameplay.Services;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace UI.Windows
{
    public class DialogueWindow : MonoBehaviour, IWindow, IPointerClickHandler
    {
        [SerializeField] private Image _avatar;
        [SerializeField] private TMP_Text _speech;
        [SerializeField] private TMP_Text _name;

        private InputService _inputService;
        private UpdateController _updateController;

        private Action _onDialogueCompleted;
        private DialogueStage[] _dialogueStages;
        private int _currentDialogueStageIndex;

        public GameObject GameObject => gameObject;

        public event EventHandler<EventArgs> OnCloseRequested;

        [Inject]
        public void Construct(InputService inputService, UpdateController updateController)
        {
            _updateController = updateController;
            _inputService = inputService;
        }

        public void Init(DialogueData dialogueData, Action onDialogueCompleted)
        {
            _dialogueStages = dialogueData.DialogueStages;
            _onDialogueCompleted = onDialogueCompleted;

            UpdateWindow();
        }

        private void OnEnable()
        {
            _inputService.LockInput();
            _updateController.Pause();
        }

        private void Update()
        {
            // TODO: add image?
            if (Input.GetKeyDown(KeyCode.E))
            {
                ProgressDialogue();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ProgressDialogue();
        }

        private void ProgressDialogue()
        {
            if (_currentDialogueStageIndex >= _dialogueStages.Length - 1)
            {
                OnCloseRequested?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                _currentDialogueStageIndex++;
                UpdateWindow();
            }
        }

        private void UpdateWindow()
        {
            var currentStage = _dialogueStages[_currentDialogueStageIndex];
            UpdateAvatar(currentStage.Avatar);
            UpdateName(currentStage.Name);
            UpdateSpeech(currentStage.Speech);
        }

        private void UpdateAvatar(Sprite avatarSprite)
        {
            var haveSprite = avatarSprite != null;
            if (haveSprite)
            {
                _avatar.sprite = avatarSprite;
            }

            _avatar.gameObject.SetActive(haveSprite);
        }

        private void UpdateName(string nameString)
        {
            var hasName = !string.IsNullOrEmpty(nameString);
            if (hasName)
            {
                _name.text = nameString;
            }

            _name.transform.parent.gameObject.SetActive(hasName);
        }

        private void UpdateSpeech(string speech)
        {
            _speech.text = speech;
        }

        private void OnDisable()
        {
            _inputService.UnlockInput();
            _updateController.Unpause();
        }

        private void OnDestroy()
        {
            _onDialogueCompleted?.Invoke();
        }
    }
}