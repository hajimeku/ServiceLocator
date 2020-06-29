using UnityEngine;

namespace Packages.ServiceLocator.Example
{
    public class ExampleGame : MonoBehaviour
    {
        //Using an arrow function here so we can hotswap the service and the game will continue using the new service
        private IExampleLog ExampleLog => ServiceLocatorManager.Instance.Resolve<IExampleLog>();
        
        // Start is called before the first frame update
        void Start()
        {
            ExampleLog.LogExample();
        }
    }
}
