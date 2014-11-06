using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Chill;
using System;

namespace Chill.Autofac
{
    internal class AutofacChillContainer : IChillContainer
    {
        private ILifetimeScope _container;
        private ContainerBuilder _containerBuilder;

        public AutofacChillContainer()
            : this(new ContainerBuilder())
        {
        }

        public AutofacChillContainer(ILifetimeScope container)
        {
            _container = container;
        }

        public AutofacChillContainer(ContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder;
        }

        protected ILifetimeScope Container
        {
            get
            {
                if (_container == null)
                    _container = _containerBuilder.Build();
                return _container;
            }
        }

        public void Dispose()
        {
            Container.Dispose();
        }


        public void RegisterType<T>()
        {
            Container.ComponentRegistry.Register(RegistrationBuilder.ForType<T>().InstancePerLifetimeScope().CreateRegistration());
        }

        public T Get<T>(string key = null) where T : class
        {
            if (key == null)
            {
                return Container.Resolve<T>();
            }
            else
            {
                return Container.ResolveKeyed<T>(key);
            }
        }

        public T Set<T>(T valueToSet, string key = null) where T : class
        {
            if (key == null)
            {
                Container.ComponentRegistry
                    .Register(RegistrationBuilder.ForDelegate((c, p) => valueToSet)
                        .InstancePerLifetimeScope().CreateRegistration());

            }
            else
            {
                Container.ComponentRegistry
                    .Register(RegistrationBuilder.ForDelegate((c, p) => valueToSet)
                        .As(new KeyedService(key, typeof(T)))
                        .InstancePerLifetimeScope().CreateRegistration());
            }
            return Get<T>();
        }


        public bool IsRegistered<T>()
        {
            return IsRegistered(typeof(T));
        }

        public bool IsRegistered(System.Type type)
        {
            return Container.IsRegistered(type);
        }
    }
}