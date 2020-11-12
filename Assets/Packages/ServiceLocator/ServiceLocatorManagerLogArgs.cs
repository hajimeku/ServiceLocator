using System;

namespace Packages.ServiceLocator
{
    public class ServiceLocatorManagerLogArgs
    {
        public ServiceLocatorManager ServiceLocatorManager { get; private set; }
        public ServiceManagerLogType LogType { get; private set; }
        /// <summary>
        /// The type registered or resolved.
        /// </summary>
        public Type Type{ get; private set;}
        /// <summary>
        /// The instance that was registered.
        /// </summary>
        public object RegisteredInstance { get; private set;}

        protected ServiceLocatorManagerLogArgs()
        {
            
        }

        public static ServiceLocatorManagerLogArgs Registered(ServiceLocatorManager serviceLocatorManager, Type type, object instance)
        {
            var args = new ServiceLocatorManagerLogArgs();
            args.ServiceLocatorManager = serviceLocatorManager;
            args.LogType = ServiceManagerLogType.Registered;
            args.Type = type;
            args.RegisteredInstance = instance;
            return args;
        }
        
        public static ServiceLocatorManagerLogArgs Missing(ServiceLocatorManager serviceLocatorManager, Type type)
        {
            var args = new ServiceLocatorManagerLogArgs();
            args.ServiceLocatorManager = serviceLocatorManager;
            args.LogType = ServiceManagerLogType.Missing;
            args.Type = type;
            return args;
        }

        public static ServiceLocatorManagerLogArgs Resetting(ServiceLocatorManager serviceLocatorManager)
        {
            var args = new ServiceLocatorManagerLogArgs();
            args.ServiceLocatorManager = serviceLocatorManager;
            args.LogType = ServiceManagerLogType.Resetting;
            return args;
        }
        
    }
}