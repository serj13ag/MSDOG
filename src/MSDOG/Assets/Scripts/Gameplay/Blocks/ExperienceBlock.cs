using System;
using Core.Services;
using UnityEngine;

namespace Gameplay.Blocks
{
    public class ExperienceBlock
    {
        private readonly int[] _experienceProgression;

        private int _currentExperience;
        private int _maxExperience;
        private int _experienceProgressionIndex;

        public int CurrentExperience => _currentExperience;
        public int MaxExperience => _maxExperience;

        public event Action OnExperienceChanged;

        public ExperienceBlock(IDataService dataService)
        {
            _experienceProgression = dataService.GetSettings().ExperienceProgression;
            _maxExperience = GetMaxExperience();
        }

        public void AddExperience(int experience)
        {
            if (experience <= 0)
            {
                return;
            }

            var newExperience = _currentExperience + experience;
            newExperience = Mathf.Min(newExperience, _maxExperience);

            SetCurrentExperience(newExperience);
        }

        public void ResetExperience()
        {
            SetCurrentExperience(0);

            if (_experienceProgressionIndex < _experienceProgression.Length - 1)
            {
                _experienceProgressionIndex++;
            }

            _maxExperience = GetMaxExperience();
        }

        private void SetCurrentExperience(int value)
        {
            if (_currentExperience != value)
            {
                _currentExperience = value;
                OnExperienceChanged?.Invoke();
            }
        }

        private int GetMaxExperience()
        {
            return _experienceProgression[_experienceProgressionIndex];
        }
    }
}