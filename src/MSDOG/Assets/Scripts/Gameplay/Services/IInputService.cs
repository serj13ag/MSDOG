using System;
using UnityEngine;

namespace Gameplay.Services
{
    public interface IInputService
    {
        Vector2 MoveInput { get; }

        event EventHandler<EventArgs> OnMenuActionPerformed;

        void LockInput();
        void UnlockInput();
    }
}