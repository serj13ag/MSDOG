using Windows;
using UnityEngine;

namespace Core.Models.Data
{
    [CreateAssetMenu(fileName = "WindowsData", menuName = "Data/WindowsData")]
    public class WindowsData : ScriptableObject
    {
        public LoseWindow LoseWindowPrefab;
        public WinWindow WinWindowPrefab;
        public OptionsWindow OptionsWindowPrefab;
        public CreditsWindow CreditsWindowPrefab;
        public DialogueWindow DialogueWindowPrefab;
        public TutorialWindow TutorialWindowPrefab;
        public EscapeWindow EscapeWindowPrefab;
    }
}