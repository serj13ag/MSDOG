using Core.Models.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class TutorialWindow : BaseCloseableWindow
    {
        [SerializeField] private Image _hintImage;
        [SerializeField] private TMP_Text _hintText;

        public void Init(TutorialEventData tutorialEventData)
        {
            _hintImage.sprite = tutorialEventData.Image;
            _hintText.text = tutorialEventData.HintText;
        }
    }
}