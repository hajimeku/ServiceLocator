using Packages.ServiceLocator;
using UnityEngine;

/// <summary>
/// Logs the LogType
/// </summary>
public class MockLogger : IServiceLocatorManagerLogger
{
    public void Log(ServiceLocatorManagerLogArgs args)
    {
        Debug.Log(args.LogType);
    }
}