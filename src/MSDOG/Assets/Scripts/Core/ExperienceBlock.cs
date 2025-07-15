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
        public bool IsFull => _currentExperience == MaxExperience;

        public event Action OnExperienceChanged;

        public void AddExperience(int experience)
        {
            if (experience <= 0)
            {
                return;
            }

            var newExperience = _currentExperience + experience;
            newExperience = Mathf.Min(newExperience, MaxExperienceConst);

            if (_currentExperience != newExperience)
            {
                _currentExperience = newExperience;
                OnExperienceChanged?.Invoke();
            }
        }
    }
}