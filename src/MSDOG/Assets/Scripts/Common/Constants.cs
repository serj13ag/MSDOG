using UnityEngine;

namespace Common
{
    public static class Constants
    {
        public static class SceneNames
        {
            public const string MenuSceneName = "Menu";
            public const string LevelSceneName = "Level";
            public const string LevelTvHudSceneName = "LevelTvHud";
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

        public static class CuttingBlowAbility
        {
            public const float BoxHeight = 2f;
            public const float BoxWidth = 1.7f;
        }
    }
}