
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace UnityLib.DI
{
    internal class CallSiteFactory
    {
        private const int DefaultSlot = 0;
        private readonly List<ServiceDescriptor> _descriptors;
        private readonly ConcurrentDictionary<Type, ServiceCallSite> _callSiteCache = new ConcurrentDictionary<Type, ServiceCallSite>();
        private readonly Dictionary<Type, ServiceDescriptorCacheItem> _descriptorLookup = new Dictionary<Type, ServiceDescriptorCacheItem>();

        private readonly StackGuard _stackGuard;

        public CallSiteFactory(IEnumerable<ServiceDescriptor> descriptors)
        {
            _stackGuard = new StackGuard();
            _descriptors = descriptors.ToList();
            Populate(descriptors);
        }

        private void Populate(IEnumerable<ServiceDescriptor> descriptors)
        {
            foreach (var descriptor in descriptors)
            {
                var serviceTypeInfo = descriptor.ServiceType.GetTypeInfo();
                if (serviceTypeInfo.IsGenericTypeDefinition)
                {
                    var implementationTypeInfo = descriptor.ImplementationType?.GetTypeInfo();

                    if (implementationTypeInfo == null || !implementationTypeInfo.IsGenericTypeDefinition)
                    {
                        throw new ArgumentException(nameof(descriptors));
                    }

                    if (implementationTypeInfo.IsAbstract || implementationTypeInfo.IsInterface)
                    {
                        throw new ArgumentException(nameof(descriptors));
                    }
                }
                else if (descriptor.ImplementationInstance == null && descriptor.ImplementationFactory == null)
                {
                    Debug.Assert(descriptor.ImplementationType != null);
                    var implementationTypeInfo = descriptor.ImplementationType.GetTypeInfo();

                    if (implementationTypeInfo.IsGenericTypeDefinition ||
                        implementationTypeInfo.IsAbstract ||
                        implementationTypeInfo.IsInterface)
                    {
                        throw new ArgumentException(nameof(descriptors));
                    }
                }

                var cacheKey = descriptor.ServiceType;
                _descriptorLookup.TryGetValue(cacheKey, out var cacheItem);
                _descriptorLookup[cacheKey] = cacheItem.Add(descriptor);
            }
        }

        internal ServiceCallSite GetCallSite(Type serviceType, CallSiteChain callSiteChain)
        {
            return _callSiteCache.GetOrAdd(serviceType, type => CreateCallSite(type, callSiteChain));
        }

        private ServiceCallSite CreateCallSite(Type serviceType, CallSiteChain callSiteChain)
        {
            if (!_stackGuard.TryEnterOnCurrentStack())
            {
                return _stackGuard.RunOnEmptyStack((type, chain) => CreateCallSite(type, chain), serviceType, callSiteChain);
            }

            ServiceCallSite callSite;
            try
            {
                callSiteChain.CheckCircularDependency(serviceType);

                callSite = TryCreateExact(serviceType, callSiteChain) ??
                           TryCreateOpenGeneric(serviceType, callSiteChain) ??
                           TryCreateEnumerable(serviceType, callSiteChain);
            }
            finally
            {
                callSiteChain.Remove(serviceType);
            }

            _callSiteCache[serviceType] = callSite;

            return callSite;
        }

        private ServiceCallSite TryCreateExact(Type serviceType, CallSiteChain callSiteChain)
        {
            if (_descriptorLookup.TryGetValue(serviceType, out var descriptor))
            {
                return TryCreateExact(descriptor.Last, serviceType, callSiteChain, DefaultSlot);
            }

            return null;
        }

        private ServiceCallSite TryCreateOpenGeneric(Type serviceType, CallSiteChain callSiteChain)
        {
            if (serviceType.IsConstructedGenericType
                && _descriptorLookup.TryGetValue(serviceType.GetGenericTypeDefinition(), out var descriptor))
            {
                return TryCreateOpenGeneric(descriptor.Last, serviceType, callSiteChain, DefaultSlot);
            }

            return null;
        }

        private ServiceCallSite TryCreateEnumerable(Type serviceType, CallSiteChain callSiteChain)
        {
            if (serviceType.IsConstructedGenericType &&
                serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var itemType = serviceType.GenericTypeArguments.Single();
                callSiteChain.Add(serviceType);

                var callSites = new List<ServiceCallSite>();

                if (!itemType.IsConstructedGenericType &&
                    _descriptorLookup.TryGetValue(itemType, out var descriptors))
                {
                    for (int i = 0; i < descriptors.Count; i++)
                    {
                        var descriptor = descriptors[i];

                        var slot = descriptors.Count - i - 1;
                        var callSite = TryCreateExact(descriptor, itemType, callSiteChain, slot);
                        Debug.Assert(callSite != null);

                        callSites.Add(callSite);
                    }
                }
                else
                {
                    var slot = 0;
                    for (var i = _descriptors.Count - 1; i >= 0; i--)
                    {
                        var descriptor = _descriptors[i];
                        var callSite = TryCreateExact(descriptor, itemType, callSiteChain, slot) ??
                                       TryCreateOpenGeneric(descriptor, itemType, callSiteChain, slot);
                        slot++;
                        if (callSite != null)
                        {
                            callSites.Add(callSite);
                        }
                    }

                    callSites.Reverse();
                }

                return new IEnumerableCallSite(itemType, callSites.ToArray());
            }

            return null;
        }

        private ServiceCallSite TryCreateExact(ServiceDescriptor descriptor, Type serviceType, CallSiteChain callSiteChain, int slot)
        {
            if (serviceType == descriptor.ServiceType)
            {
                ServiceCallSite callSite;
                var lifetime = new ResultCache(descriptor.Lifetime, serviceType, slot);
                if (descriptor.ImplementationInstance != null)
                {
                    callSite = new ConstantCallSite(descriptor.ServiceType, descriptor.ImplementationInstance);
                }
                else if (descriptor.ImplementationFactory != null)
                {
                    callSite = new FactoryCallSite(lifetime, descriptor.ServiceType, descriptor.ImplementationFactory);
                }
                else if (descriptor.ImplementationType != null)
                {
                    callSite = CreateConstructorCallSite(lifetime, descriptor.ServiceType, descriptor.ImplementationType, callSiteChain);
                }
                else
                {
                    throw new InvalidOperationException("Invalid service descriptor");
                }

                return callSite;
            }

            return null;
        }

        private ServiceCallSite TryCreateOpenGeneric(ServiceDescriptor descriptor, Type serviceType, CallSiteChain callSiteChain, int slot)
        {
            if (serviceType.IsConstructedGenericType &&
                serviceType.GetGenericTypeDefinition() == descriptor.ServiceType)
            {
                Debug.Assert(descriptor.ImplementationType != null, "descriptor.ImplementationType != null");
                var lifetime = new ResultCache(descriptor.Lifetime, serviceType, slot);
                var closedType = descriptor.ImplementationType.MakeGenericType(serviceType.GenericTypeArguments);
                return CreateConstructorCallSite(lifetime, serviceType, closedType, callSiteChain);
            }

            return null;
        }

        private ServiceCallSite CreateConstructorCallSite(ResultCache lifetime, Type serviceType, Type implementationType,CallSiteChain callSiteChain)
        {
            callSiteChain.Add(serviceType, implementationType);

            var constructors = implementationType.GetTypeInfo()
                .DeclaredConstructors
                .Where(constructor => constructor.IsPublic)
                .ToArray();

            ServiceCallSite[] parameterCallSites = null;

            if (constructors.Length == 0)
            {
                throw new InvalidOperationException(nameof(implementationType));
            }
            else if (constructors.Length == 1)
            {
                var constructor = constructors[0];
                var parameters = constructor.GetParameters();
                if (parameters.Length == 0)
                {
                    return new ConstructorCallSite(lifetime, serviceType, constructor);
                }

                parameterCallSites = CreateArgumentCallSites(
                    serviceType,
                    implementationType,
                    callSiteChain,
                    parameters,
                    throwIfCallSiteNotFound: true);

                return new ConstructorCallSite(lifetime, serviceType, constructor, parameterCallSites);
            }

            Array.Sort(constructors,
                (a, b) => b.GetParameters().Length.CompareTo(a.GetParameters().Length));

            ConstructorInfo bestConstructor = null;
            HashSet<Type> bestConstructorParameterTypes = null;
            for (var i = 0; i < constructors.Length; i++)
            {
                var parameters = constructors[i].GetParameters();

                var currentParameterCallSites = CreateArgumentCallSites(
                    serviceType,
                    implementationType,
                    callSiteChain,
                    parameters,
                    throwIfCallSiteNotFound: false);

                if (currentParameterCallSites != null)
                {
                    if (bestConstructor == null)
                    {
                        bestConstructor = constructors[i];
                        parameterCallSites = currentParameterCallSites;
                    }
                    else
                    {
                        // Since we're visiting constructors in decreasing order of number of parameters,
                        // we'll only see ambiguities or supersets once we've seen a 'bestConstructor'.

                        if (bestConstructorParameterTypes == null)
                        {
                            bestConstructorParameterTypes = new HashSet<Type>(
                                bestConstructor.GetParameters().Select(p => p.ParameterType));
                        }

                        if (!bestConstructorParameterTypes.IsSupersetOf(parameters.Select(p => p.ParameterType)))
                        {
                            // Ambiguous match exception
                            var message = $"{implementationType}????????????????????????!";
                            throw new InvalidOperationException(message);
                        }
                    }
                }
            }

            if (bestConstructor == null)
            {
                throw new InvalidOperationException($"{implementationType}??????????????????");
            }
            else
            {
                Debug.Assert(parameterCallSites != null);
                return new ConstructorCallSite(lifetime, serviceType, bestConstructor, parameterCallSites);
            }
        }

        private ServiceCallSite[] CreateArgumentCallSites(
            Type serviceType,
            Type implementationType,
            CallSiteChain callSiteChain,
            ParameterInfo[] parameters,
            bool throwIfCallSiteNotFound)
        {
            var parameterCallSites = new ServiceCallSite[parameters.Length];
            for (var index = 0; index < parameters.Length; index++)
            {
                var callSite = GetCallSite(parameters[index].ParameterType, callSiteChain);

                if (callSite == null && ParameterDefaultValue.TryGetDefaultValue(parameters[index], out var defaultValue))
                {
                    callSite = new ConstantCallSite(serviceType, defaultValue);
                }

                if (callSite == null)
                {
                    if (throwIfCallSiteNotFound)
                    {
                        throw new InvalidOperationException($"{implementationType}??????????????????????????????{parameters[index].ParameterType}");
                    }

                    return null;
                }

                parameterCallSites[index] = callSite;
            }

            return parameterCallSites;
        }


        public void Add(Type type, ServiceCallSite serviceCallSite)
        {
            _callSiteCache[type] = serviceCallSite;
        }

        private struct ServiceDescriptorCacheItem
        {
            private ServiceDescriptor _item;
            private List<ServiceDescriptor> _items;

            public ServiceDescriptor Last
            {
                get
                {
                    if (_items != null && _items.Count > 0)
                    {
                        return _items[_items.Count - 1];
                    }

                    Debug.Assert(_item != null);
                    return _item;
                }
            }

            public int Count
            {
                get
                {
                    if (_item == null)
                    {
                        Debug.Assert(_items == null);
                        return 0;
                    }

                    return 1 + (_items?.Count ?? 0);
                }
            }

            public ServiceDescriptor this[int index]
            {
                get
                {
                    if (index >= Count)
                    {
                        throw new ArgumentOutOfRangeException(nameof(index));
                    }

                    if (index == 0)
                    {
                        return _item;
                    }

                    return _items[index - 1];
                }
            }

            public ServiceDescriptorCacheItem Add(ServiceDescriptor descriptor)
            {
                var newCacheItem = new ServiceDescriptorCacheItem();
                if (_item == null)
                {
                    Debug.Assert(_items == null);
                    newCacheItem._item = descriptor;
                }
                else
                {
                    newCacheItem._item = _item;
                    newCacheItem._items = _items ?? new List<ServiceDescriptor>();
                    newCacheItem._items.Add(descriptor);
                }
                return newCacheItem;
            }
        }
    }
}
