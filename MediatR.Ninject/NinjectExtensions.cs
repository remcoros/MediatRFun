using System;
using MediatR.Ninject.Configuration;
using Ninject;
using Ninject.Extensions.Conventions.Syntax;

namespace MediatR.Ninject
{
    public static class NinjectExtensions
    {
        public static MediatorConfiguration AddMediatR(this IKernel kernel, Func<IFromSyntax, ISelectSyntax> from, Action<MediatorConfiguration> configuration = null)
        {
            var config = new MediatorConfiguration(kernel, from);
            config.Configure();
            configuration?.Invoke(config);

            return config;
        }
    }
}