using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Planning.Bindings;
using Ninject.Planning.Bindings.Resolvers;

namespace MediatR.Ninject.Resolvers
{
    public class CovariantBindingResolver : NinjectComponent, IBindingResolver
    {
        /// <summary>
        /// Returns any bindings from the specified collection that match the specified service.
        /// </summary>
        public IEnumerable<IBinding> Resolve(Multimap<Type, IBinding> bindings, Type service)
        {
            if (service.IsGenericType)
            {
                var genericType = service.GetGenericTypeDefinition();
                var genericArguments = genericType.GetGenericArguments();
                if (genericArguments.Length == 1
                    && genericArguments.Single().GenericParameterAttributes.HasFlag(GenericParameterAttributes.Covariant))
                {
                    var argument = service.GetGenericArguments()[0];
                    var matches = bindings.Where(kvp => kvp.Key.IsGenericType
                                                        && kvp.Key.GetGenericTypeDefinition() == genericType
                                                        && argument.IsAssignableFrom(kvp.Key.GetGenericArguments()[0]))
                        .SelectMany(kvp => kvp.Value);
                    return matches;
                }

                if (genericArguments.Length == 2
                    && genericArguments[0].GenericParameterAttributes.HasFlag(GenericParameterAttributes.Covariant)
                    && genericArguments[1].GenericParameterAttributes.HasFlag(GenericParameterAttributes.Covariant))
                {
                    var arg0 = service.GetGenericArguments()[0];
                    var arg1 = service.GetGenericArguments()[1];
                    var matches = bindings.Where(kvp => kvp.Key.IsGenericType
                                                        && kvp.Key.GetGenericTypeDefinition() == genericType
                                                        && arg0.IsAssignableFrom(kvp.Key.GetGenericArguments()[0])
                                                        && arg1.IsAssignableFrom(kvp.Key.GetGenericArguments()[1]))
                        .SelectMany(kvp => kvp.Value);
                    return matches;
                }
            }

            return Enumerable.Empty<IBinding>();
        }
    }
}