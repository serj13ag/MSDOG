using System;
using UnityEngine;

namespace Gameplay.Services
{
    public interface IInputService
    {
        Vector2 MoveInput { get; }
        bool IsClickDown { get; }
        bool IsClickUp { get; }

        event EventHandler<EventArgs> OnMenuActionPerformed;

        void LockInput();
        void UnlockInput();
    }
}