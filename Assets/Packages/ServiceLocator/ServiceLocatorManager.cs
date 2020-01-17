using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public static class ServiceLocatorManager
{
    private static readonly Dictionary<Type, object> Services = new Dictionary<Type, object>();
    
    public static void Register<T>(object serviceInstance)
    {
        Services[typeof(T)] = serviceInstance;
    }

    public static T Resolve<T>()
    {
        Type type = typeof(T);
        if (!Services.ContainsKey(type))
        {
            ServiceLocatorErrorLog("Cant find Service of type: " + type.Name);
            return default;
        }

        return (T)Services[type];
    }

    public static void Reset()
    {
        foreach (KeyValuePair<Type,object> keyValuePair in Services)
        {
            if (keyValuePair.Value is MonoBehaviour behaviour)
            {
                Object.Destroy(behaviour.gameObject);
            }
        }
        Services.Clear();
    }

    public static void ServiceLocatorErrorLog(string log)
    {
        string logMsg = string.Concat("ServiceLocator - ", log);
        Debug.LogError(logMsg);
    }
    
    public static Component AsMono<T>()
    {
        GameObject gameObject = new GameObject(typeof(T).Name);
        var obj = gameObject.AddComponent(typeof(T));
        return obj;
    }
}
