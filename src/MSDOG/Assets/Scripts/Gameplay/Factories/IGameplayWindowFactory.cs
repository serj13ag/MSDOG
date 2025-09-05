using System;
using Windows;
using Core.Models.Data;
using UnityEngine;

namespace Gameplay.Factories
{
    public interface IGameplayWindowFactory
    {
        IWindow CreateLoseWindow(Transform canvasTransform);
        IWindow CreateWinWindow(Transform canvasTransform);
        IWindow CreateEscapeWindow(Transform canvasTransform);
        IWindow CreateDialogueWindow(DialogueData dialogueData, Action onDialogueCompleted, Transform canvasTransform);
        IWindow CreateTutorialWindow(TutorialEventData tutorialEventData, Transform canvasTransform);
    }
}