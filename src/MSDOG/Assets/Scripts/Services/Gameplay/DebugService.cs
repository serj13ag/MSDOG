using Infrastructure;
using UI.HUD;
using UnityEngine;

namespace Services.Gameplay
{
    public class DebugService : MonoBehaviour
    {
        private UpdateService _updateService;
        private HudController _hudController;

        private bool _isActive;

        public void Init(UpdateService updateService)
        {
            _updateService = updateService;
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

            // if (GUILayout.Button("Pause/Unpause", style))
            // {
            //     if (_updateService.IsPaused)
            //     {
            //         _updateService.Unpause();
            //     }
            //     else
            //     {
            //         _updateService.Pause();
            //     }
            // }

            GUILayout.Label("ABILITIES");

            var abilities = GlobalServices.DataService.GetAbilitiesData();
            foreach (var abilityData in abilities)
            {
                if (GUILayout.Button(abilityData.name, style))
                {
                    _hudController.AddAbility(abilityData);
                }
            }

            GUILayout.EndVertical();
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