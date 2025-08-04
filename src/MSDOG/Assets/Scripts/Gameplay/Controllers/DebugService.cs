using System;
using Core.Controllers;
using Core.Services;
using UI.HUD;
using UnityEngine;
using VContainer;

namespace Gameplay.Controllers
{
    public class DebugService : MonoBehaviour
    {
        private UpdateController _updateController;
        private DataService _dataService;

        private HudController _hudController;

        private bool _isActive;

        public bool DebugHpIsVisible { get; private set; }

        public event EventHandler<EventArgs> OnShowDebugHealthBar;
        public event EventHandler<EventArgs> OnHideDebugHealthBar;
        public event EventHandler<EventArgs> OnForceSpawnEnemiesRequested;
        public event EventHandler<EventArgs> OnKillAllEnemiesRequested;

        [Inject]
        public void Construct(UpdateController updateController, DataService dataService)
        {
            _dataService = dataService;
            _updateController = updateController;
        }

        public void Setup(HudController hudController)
        {
            _hudController = hudController;
        }

        public void OnGUI()
        {
            if (!_isActive)
            {
                return;
            }

            DrawGui();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F12))
            {
                _isActive = !_isActive;
            }
        }

        private void DrawGui()
        {
            var h = Screen.height;
            var style = new GUIStyle
            {
                alignment = TextAnchor.UpperLeft,
                fontSize = h * 2 / 100,
                normal =
                {
                    textColor = new Color(1f, 1f, 1f, 1.0f),
                    background = CreateTexture(1, 1, new Color(0f, 0f, 0f, 0.4f)),
                },
            };

            GUILayout.BeginVertical(style);

            GUILayout.Label("GENERAL");
            DrawGeneral(style);

            GUILayout.Label("ABILITIES");
            DrawAbilities(style);

            GUILayout.EndVertical();
        }

        private void DrawGeneral(GUIStyle style)
        {
            if (GUILayout.Button("Pause/Unpause", style))
            {
                if (_updateController.IsPaused)
                {
                    _updateController.Unpause();
                }
                else
                {
                    _updateController.Pause();
                }
            }

            if (GUILayout.Button("Show Debug HP", style))
            {
                if (DebugHpIsVisible)
                {
                    DebugHpIsVisible = false;
                    OnHideDebugHealthBar?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    DebugHpIsVisible = true;
                    OnShowDebugHealthBar?.Invoke(this, EventArgs.Empty);
                }
            }

            if (GUILayout.Button("Force spawn", style))
            {
                OnForceSpawnEnemiesRequested?.Invoke(this, EventArgs.Empty);
            }

            if (GUILayout.Button("Kill enemies", style))
            {
                OnKillAllEnemiesRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        private void DrawAbilities(GUIStyle style)
        {
            var abilities = _dataService.GetAbilitiesData();
            foreach (var abilityData in abilities)
            {
                if (GUILayout.Button(abilityData.name, style))
                {
                    _hudController.AddAbility(abilityData);
                }
            }
        }

        private static Texture2D CreateTexture(int width, int height, Color color)
        {
            var pix = new Color[width * height];

            for (var i = 0; i < pix.Length; i++)
            {
                pix[i] = color;
            }

            var result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }
    }
}