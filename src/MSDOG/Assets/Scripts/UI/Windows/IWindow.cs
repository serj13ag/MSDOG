using System;
using UnityEngine;

namespace UI.Windows
{
    public interface IWindow
    {
        GameObject GameObject { get; }

        event EventHandler<EventArgs> OnCloseRequested;
    }
}