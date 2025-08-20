using System;

namespace Gameplay
{
    public class DetailCreatedEventArgs : EventArgs
    {
        public Detail Detail { get; }

        public DetailCreatedEventArgs(Detail detail)
        {
            Detail = detail;
        }
    }
}