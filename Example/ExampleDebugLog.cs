using UnityEngine;

namespace Packages.ServiceLocator.Example
{
    public class ExampleDebugLog : IExampleLog
    {
        public void LogExample()
        {
            Debug.Log("Log Debug Class");
        }
    }
}