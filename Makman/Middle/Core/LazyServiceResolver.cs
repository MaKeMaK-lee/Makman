using Microsoft.Extensions.DependencyInjection;

namespace Makman.Middle.Core
{
    public static class LazyServiceResolver
    {
        public static IServiceCollection AddLazyResolution(this IServiceCollection services)
        {
            return services.AddTransient(
              typeof(Lazy<>),
              typeof(LazilyResolved<>));
        }

        private class LazilyResolved<T> : Lazy<T>
        {
            public LazilyResolved(IServiceProvider serviceProvider)
                : base(serviceProvider.GetRequiredService<T>) { }//TODO WHY
        }
    }
}
