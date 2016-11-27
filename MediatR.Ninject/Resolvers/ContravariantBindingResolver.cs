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
    public class ContravariantBindingResolver : NinjectComponent, IBindingResolver
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
                    && genericArguments.Single().GenericParameterAttributes.HasFlag(GenericParameterAttributes.Contravariant))
                {
                    var argument = service.GetGenericArguments()[0];
                    var matches = bindings.Where(kvp => kvp.Key.IsGenericType
                                                        && kvp.Key.GetGenericTypeDefinition() == genericType
                                                        && kvp.Key.GetGenericArguments()[0] != argument
                                                        && kvp.Key.GetGenericArguments()[0].IsAssignableFrom(argument))
                        .SelectMany(kvp => kvp.Value);
                    return matches;
                }

                if (genericArguments.Length == 2
                    && genericArguments[0].GenericParameterAttributes.HasFlag(GenericParameterAttributes.Contravariant)
                    && genericArguments[1].GenericParameterAttributes.HasFlag(GenericParameterAttributes.Contravariant))
                {
                    var arg0 = service.GetGenericArguments()[0];
                    var arg1 = service.GetGenericArguments()[1];
                    var matches = bindings.Where(kvp => kvp.Key.IsGenericType
                                                        && kvp.Key.GetGenericTypeDefinition() == genericType
                                                        && kvp.Key.GetGenericArguments()[0] != arg0
                                                        && kvp.Key.GetGenericArguments()[0].IsAssignableFrom(arg0)
                                                        && kvp.Key.GetGenericArguments()[1].IsAssignableFrom(arg1))
                        .SelectMany(kvp => kvp.Value);
                    return matches;
                }

                if (genericArguments.Length == 2
                    && genericArguments[0].GenericParameterAttributes.HasFlag(GenericParameterAttributes.Contravariant)
                    && genericArguments[1].GenericParameterAttributes.HasFlag(GenericParameterAttributes.Covariant))
                {
                    var arg0 = service.GetGenericArguments()[0];
                    var arg1 = service.GetGenericArguments()[1];
                    var matches = bindings.Where(kvp => kvp.Key.IsGenericType
                                                        && kvp.Key.GetGenericTypeDefinition() == genericType
                                                        && kvp.Key.GetGenericArguments()[0] != arg0
                                                        && kvp.Key.GetGenericArguments()[0].IsAssignableFrom(arg0)
                                                        && kvp.Key.GetGenericArguments()[1].IsAssignableFrom(arg1))
                        .SelectMany(kvp => kvp.Value)
                        .ToList();
                    return matches;
                }
            }

            return Enumerable.Empty<IBinding>();
        }
    }
}