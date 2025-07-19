namespace Core.Abilities
{
    public interface IAbility
    {
        void Activate();
        void OnUpdate(float deltaTime);
        void Deactivate();
    }
}