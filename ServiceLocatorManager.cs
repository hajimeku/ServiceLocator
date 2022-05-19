using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Packages.ServiceLocator
{
    public class ServiceLocatorManager
    {
        private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();
        private static ServiceLocatorManager instance;

        public ServiceLocatorManager()
        {
            RegisterDefaultLogger();
        }

        private IServiceLocatorManagerLogger Logger
        {
            get
            {
                if (services.TryGetValue(typeof(IServiceLocatorManagerLogger), out var loggerObject))
                {
                    return loggerObject as IServiceLocatorManagerLogger;
                }

                return default;
            }
        }

        private void RegisterDefaultLogger()
        {
            Register<IServiceLocatorManagerLogger>(new ServiceLocatorDefaultLogger());
        }

        public void Register<T>(object serviceInstance)
        {
            services[typeof(T)] = serviceInstance;
            
            Logger?.Log(ServiceLocatorManagerLogArgs.Registered(this, typeof(T), serviceInstance));
        }
        
        public void Register<T>(ServiceLocatorPrefab serviceInstance,bool destroyOnLoad = true)
        {
            if (serviceInstance == null || serviceInstance.ObjectToInstantiate == null) 
            {
                return;
            }

            Transform parent = serviceInstance.Parent;
            
            GameObject objectToInstantiate = Object.Instantiate(serviceInstance.ObjectToInstantiate, parent);
            
            var mainService = objectToInstantiate.GetComponent<T>();
            
            if (!destroyOnLoad)
            {
                Object.DontDestroyOnLoad(objectToInstantiate);
            }
            
            services[typeof(T)] = mainService;
            
            Logger?.Log(ServiceLocatorManagerLogArgs.Registered(this, typeof(T), mainService));
        }

        public T Resolve<T>()
        {
            Type type = typeof(T);
            if (services.TryGetValue(type, out var result))
            {
                return (T) result;
            }
            
            Logger?.Log(ServiceLocatorManagerLogArgs.Missing(this, type));

            return default;
        }

        public void Reset()
        {
            Logger?.Log(ServiceLocatorManagerLogArgs.Resetting(this));
            foreach (KeyValuePair<Type,object> keyValuePair in services)
            {
                if (keyValuePair.Value is MonoBehaviour behaviour)
                {
                    Object.DestroyImmediate(behaviour.gameObject);
                }
            }
            services.Clear();
            RegisterDefaultLogger();
        }
        
        public static T AsMono<T>(bool destroyOnLoad = false) where T : Component
        {
            GameObject gameObject = new GameObject(typeof(T).Name);
            var obj = gameObject.AddComponent<T>();
            if (!destroyOnLoad)
            {
                Object.DontDestroyOnLoad(gameObject);
            }
            return obj;
        }

        public bool ServiceExists<T>()
        {
            services.TryGetValue(typeof(T), out var result);
            return result != null;
        }

        public static ServiceLocatorManager Instance
        {
            get => instance ??= new ServiceLocatorManager();
            set => instance = value;
        }
    }
}
