using System;
using UnityEngine;

namespace Core
{
    public class ExperienceBlock
    {
        private const int MaxExperienceConst = 10;

        private int _currentExperience;

        public int CurrentExperience => _currentExperience;
        public int MaxExperience => MaxExperienceConst;

        public event Action OnExperienceChanged;

        public void AddExperience(int experience)
        {
            if (experience <= 0)
            {
                return;
            }

            var newExperience = _currentExperience + experience;
            newExperience = Mathf.Min(newExperience, MaxExperienceConst);

            SetCurrentExperience(newExperience);
        }

        public void ResetExperience()
        {
            SetCurrentExperience(0);
        }

        private void SetCurrentExperience(int value)
        {
            if (_currentExperience != value)
            {
                _currentExperience = value;
                OnExperienceChanged?.Invoke();
            }
        }
    }
}