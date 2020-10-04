using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelegatesLambdasEvents
{
    class DependencyContainer
    {
        public List<Dependency> _dependencies;
        
        public DependencyContainer()
        {
          _dependencies = new List<Dependency>();
        }

        //public void AddDependency(Type type)
        //{
        //    _dependencies.Add(type);
        //}

        //public void AddDependency<T>()
        //{
        //    _dependencies.Add(typeof(T));
        //}

        public void AddSingleton<T>()
        {
            _dependencies.Add(new Dependency(typeof(T),DependencyLifetime.Singleton));
        }

        public void AddTransient<T>()
        {
            _dependencies.Add(new Dependency(typeof(T), DependencyLifetime.Transient));
        }
        public Dependency GetDependency(Type t)
        {
            return _dependencies.First(x => x.Type.Name == t.Name);
        }
    }

    class Dependency
    {
        public Dependency(Type type, DependencyLifetime lifetime)
        {
            Type = type;
            Lifetime = lifetime;

        }
        public Type Type { get; set; }
        public DependencyLifetime Lifetime { get; set; }

        public void AddImplementation(object i)
        {
            Implementation = i;
            Implemented = true;
        }
        public object Implementation { get; set; }
        public bool Implemented { get; set; }
    }
    class ServiceConsumer
    {
        public HelloService _service;
        public string _name;
        public ServiceConsumer(HelloService service)
        {
            _service = service;
        }

        public void Print()
        {
            _service.Print();
        }
    }

    class HelloService
    {
        MessageService _message;
        int _random;
        public HelloService(MessageService message)
        {
            _random = new Random().Next();
            _message = message;
        }
        public int PrintRandom()
        {
            return _random;
        }
        public void Print()
        {
            Console.WriteLine($"Hello world from hello service {PrintRandom()}");
            Console.WriteLine($"Hello world from message service: {_message.Message()}");
        }
    }

    class MessageService
    {

        int _random;
        public MessageService()
        {
            _random = new Random().Next();
        }
        
        public string Message()
        {
            return "Yo #"+_random;
        }
    }

    public enum DependencyLifetime
    {
        Singleton = 0,
        Transient = 1,
    }
    class DependencyResolver
    {
        DependencyContainer _container;
        
        public DependencyResolver(DependencyContainer container)
        {
            _container = container;
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        public object GetService(Type type)
        {
            //Get type of service
            var dependency = _container.GetDependency(type);
            //Get ctors
            var ctors = dependency.Type.GetConstructors().Single();
            //Get ctors params
            var parameters = ctors.GetParameters().ToArray();


            if (parameters.Length > 0)
            {
                //Store implementation of params
                var parametersImplementation = new Object[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    //Line below will returns you ParameterInfo[] type, we want to type of parameter of this array
                    //parametersImplementation[i] = Activator.CreateInstance(parameters[i].GetType());
                    parametersImplementation[i] = GetService(parameters[i].ParameterType);
                }
                return CreateImplementation(dependency, t=>Activator.CreateInstance(t,parametersImplementation));
            }

            return CreateImplementation(dependency, t => Activator.CreateInstance(t));

        }

        public object CreateImplementation(Dependency dependency, Func<Type,object> factory)
        {

            if (dependency.Implemented)
            {
                return dependency.Implementation;
            }

            var implementation = factory(dependency.Type);

            if (dependency.Lifetime == DependencyLifetime.Singleton)
            {
                dependency.AddImplementation(implementation);
                dependency.Implemented = true;
            }

            return implementation;
        }

    }
}
