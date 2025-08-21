using System;
using UnityEngine;

namespace Windows
{
    public interface IWindow
    {
        GameObject GameObject { get; }

        event EventHandler<EventArgs> OnCloseRequested;
    }
}