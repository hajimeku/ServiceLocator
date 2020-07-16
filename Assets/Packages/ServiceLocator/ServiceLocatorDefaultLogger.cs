using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Packages.ServiceLocator
{
    /// <summary>
    /// Default Logging service.
    /// Logs to Debug.LogError when there is a missing service.
    /// </summary>
    public class ServiceLocatorDefaultLogger : IServiceLocatorManagerLogger
    {
        public void Log(ServiceLocatorManagerLogArgs args)
        {
            switch (args.LogType)
            {
                case ServiceManagerLogType.Registered:
                    OnRegistered(args);
                    break;
                case ServiceManagerLogType.Missing:
                    OnMissing(args);
                    break;
                case ServiceManagerLogType.Resetting:
                    OnReset(args);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public virtual void OnRegistered(ServiceLocatorManagerLogArgs args)
        {
            Assert.AreEqual(args.LogType, ServiceManagerLogType.Registered);
            Assert.IsNotNull(args.Type);
        }
        
        public virtual void OnMissing(ServiceLocatorManagerLogArgs args)
        {
            Assert.AreEqual(args.LogType, ServiceManagerLogType.Missing);
            Assert.IsNotNull(args.Type);
            Assert.IsNull(args.RegisteredInstance);
            
            string logMsg = string.Concat("ServiceLocator - ", "Cant find Service of type: ", args.Type.Name);
            Debug.LogError(logMsg);
        }

        public virtual void OnReset(ServiceLocatorManagerLogArgs args)
        {
            Assert.AreEqual(args.LogType, ServiceManagerLogType.Resetting);
            Assert.IsNull(args.Type);
            Assert.IsNull(args.RegisteredInstance);
        }
    }
}