namespace Gameplay.Interfaces
{
    public interface IEntityWithDamageReduction
    {
        int CurrentDamageReductionPercent { get; }

        void ChangeDamageReductionPercent(int damageReductionPercent);
    }
}