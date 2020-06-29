using UnityEngine;

namespace Packages.ServiceLocator.Example
{
    public class ExampleProductionLog : IExampleLog
    {
        public void LogExample()
        {
            Debug.Log("Log Production Class");
        }
    }
}