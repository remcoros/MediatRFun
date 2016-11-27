using System.Linq;
using MediatR.DomainEvents;
using MediatR.Ninject.Configuration;
using MediatR.Pipeline;
using Ninject;

namespace MediatR.Ninject
{
    public static class MediatorConfigurationExtensions
    {
        public static MediatorConfiguration AddDomainEvents(this MediatorConfiguration configuration)
        {
            configuration.Kernel.Bind(typeof(IRequestHandler<,>))
                .ToMethod(c =>
                    {
                        if (c.GenericArguments?.Length == 2)
                        {
                            var i = c.GenericArguments[0].GetInterfaces().FirstOrDefault(bi => bi.IsGenericType && bi.GetGenericTypeDefinition() == typeof(ICommand<>));
                            if (i != null)
                            {
                                return c.Kernel.Get(typeof(CommandHandler<,,>).MakeGenericType(c.GenericArguments[0], c.GenericArguments[1], i.GenericTypeArguments[0]));
                            }
                        }

                        return null;
                    }).WhenInjectedInto(typeof(RequestPipeline<,>)); ;
            return configuration;
        }
    }
}