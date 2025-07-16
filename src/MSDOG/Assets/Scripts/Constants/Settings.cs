using UnityEngine;

namespace Constants
{
    public static class Settings
    {
        public static class SceneNames
        {
            public const string MenuSceneName = "Menu";
            public const string LevelSceneName = "Level";
        }

        public static class Arena
        {
            public const int Size = 40;
        }

        public static class ScreenFader
        {
            public const float SolidAlpha = 1;
            public const float ClearAlpha = 0;
            public const float Delay = 1f;
            public const float TimeToFade = 1f;
        }

        public static class LayerMasks
        {
            public static readonly int EnemyLayer = LayerMask.GetMask("Enemy");
        }

        public static class Enemy
        {
            public const float RangeCloseDistance = 5f;
            public const float RangeCloseDistanceOut = RangeCloseDistance + 1f;
        }
    }
}