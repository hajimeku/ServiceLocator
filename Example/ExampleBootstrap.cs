using UnityEngine;

namespace Packages.ServiceLocator.Example
{
    public static class ExampleBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void ConfigureServices()
        {
            //Register this for debug log
            ServiceLocatorManager.Instance.Register<IExampleLog>(new ExampleProductionLog());
            
            //Register this for a Debug log
            //ServiceLocatorManager.Instance.Register<IExampleLog>(new ExampleDebugLog());
        }
    }
}
