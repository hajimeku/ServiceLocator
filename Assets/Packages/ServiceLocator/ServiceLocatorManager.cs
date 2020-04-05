using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ServiceLocator
{
    public class ServiceLocatorManager
    {
        private readonly Dictionary<Type, object> Services = new Dictionary<Type, object>();
    
        public void Register<T>(object serviceInstance)
        {
            Services[typeof(T)] = serviceInstance;
        }

        public T Resolve<T>()
        {
            Type type = typeof(T);
            if (Services.TryGetValue(type, out var result))
            {
                return (T) result;
            }
            ServiceLocatorErrorLog("Cant find Service of type: " + type.Name);
            return default;
        }

        public void Reset()
        {
            foreach (KeyValuePair<Type,object> keyValuePair in Services)
            {
                if (keyValuePair.Value is MonoBehaviour behaviour)
                {
                    Object.DestroyImmediate(behaviour.gameObject);
                }
            }
            Services.Clear();
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

        public static void Initialize()
        {
            Instance = new ServiceLocatorManager();
        }

        public static ServiceLocatorManager Instance;
    }
}
