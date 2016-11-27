using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using MediatR.Ninject.Configuration;
using Ninject;

namespace MediatR.Ninject
{
    public static class NinjectExtensions
    {
        public static void AddMediatR(this IKernel kernel, params Assembly[] assemblies)
        {
            if (assemblies == null)
                throw new ArgumentNullException(nameof(assemblies));

            var config = new MediatorConfiguration(kernel);
            config.Configure();
            config.BindHandlers(x => x.From(assemblies));
        }

        public static void AddMediatR(this IKernel kernel, Action<MediatorConfiguration> configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var config = new MediatorConfiguration(kernel);
            config.Configure();
            configuration(config);
        }
    }
}