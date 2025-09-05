namespace Gameplay
{
    public interface IEntityWithNitro
    {
        bool HasNitro { get; }

        void SetNitro(float nitroMultiplier);
        void ResetNitro();
    }
}