using System;
using Core.Models.Data;
using UI.Windows;
using UnityEngine;

namespace Gameplay.Factories
{
    public interface IGameplayWindowFactory
    {
        LoseWindow CreateLoseWindow(Transform canvasTransform);
        WinWindow CreateWinWindow(Transform canvasTransform);
        EscapeWindow CreateEscapeWindow(Transform canvasTransform);
        IWindow CreateDialogueWindow(DialogueData dialogueData, Action onDialogueCompleted, Transform canvasTransform);
        IWindow CreateTutorialWindow(TutorialEventData tutorialEventData, Transform canvasTransform);
    }
}