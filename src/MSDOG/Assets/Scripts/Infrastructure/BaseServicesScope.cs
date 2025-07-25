using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public abstract class BaseServicesScope : LifetimeScope
    {
        protected override void Awake()
        {
            // Skip container building
        }

        public void BuildContainer()
        {
            Build();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            ConfigureContainer(builder);
        }

        protected abstract void ConfigureContainer(IContainerBuilder builder);
    }
}