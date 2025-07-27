using System;
using Services;
using UnityEngine;

namespace Core
{
    public class ExperienceBlock
    {
        private readonly DataService _dataService;

        private int _currentExperience;
        private int _maxExperience;
        private int _experienceProgressionIndex;

        public int CurrentExperience => _currentExperience;
        public int MaxExperience => _maxExperience;

        public event Action OnExperienceChanged;

        public ExperienceBlock(DataService dataService)
        {
            _dataService = dataService;
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

            if (_experienceProgressionIndex < _dataService.GetSettingsData().ExperienceProgression.Length - 1)
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
            return _dataService.GetSettingsData().ExperienceProgression[_experienceProgressionIndex];
        }
    }
}