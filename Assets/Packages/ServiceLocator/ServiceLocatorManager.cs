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

        public void Register<T>(object serviceInstance)
        {
            services[typeof(T)] = serviceInstance;
        }

        public T Resolve<T>()
        {
            Type type = typeof(T);
            if (services.TryGetValue(type, out var result))
            {
                return (T) result;
            }
            ServiceLocatorErrorLog("Cant find Service of type: " + type.Name);
            return default;
        }

        public void Reset()
        {
            foreach (KeyValuePair<Type,object> keyValuePair in services)
            {
                if (keyValuePair.Value is MonoBehaviour behaviour)
                {
                    Object.DestroyImmediate(behaviour.gameObject);
                }
            }
            services.Clear();
        }

        private void ServiceLocatorErrorLog(string log)
        {
            string logMsg = string.Concat("ServiceLocator - ", log);
            Debug.LogError(logMsg);
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

        public static ServiceLocatorManager Instance
        {
            get => instance ?? (instance = new ServiceLocatorManager());
            set => instance = value;
        }
    }
}
