using System;
using Core.Models.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Windows
{
    public class DialogueWindow : BaseWindow, IPointerClickHandler
    {
        [SerializeField] private Image _avatar;
        [SerializeField] private TMP_Text _speech;
        [SerializeField] private TMP_Text _name;

        private Action _onDialogueCompleted;
        private DialogueStage[] _dialogueStages;
        private int _currentDialogueStageIndex;

        public void Init(DialogueData dialogueData, Action onDialogueCompleted)
        {
            _dialogueStages = dialogueData.DialogueStages;
            _onDialogueCompleted = onDialogueCompleted;

            UpdateWindow();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ProgressDialogue();
        }

        private void ProgressDialogue()
        {
            if (_currentDialogueStageIndex >= _dialogueStages.Length - 1)
            {
                Close();
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

        private void OnDestroy()
        {
            _onDialogueCompleted?.Invoke();
        }
    }
}