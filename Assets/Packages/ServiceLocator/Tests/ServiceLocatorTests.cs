using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Packages.ServiceLocator;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests
{
    public class ServiceLocatorTests
    {
        private ServiceLocatorManager serviceLocatorManager;

        [SetUp]
        public void SetupTest()
        {
            serviceLocatorManager = new ServiceLocatorManager();
        }
        
        [Test]
        public void CreateNonMonoServiceAndResolveIt()
        {
            serviceLocatorManager.Register<IMockInterface>(new MockServiceOldFeature());

            IMockInterface oldService = serviceLocatorManager.Resolve<IMockInterface>();
            Assert.IsInstanceOf(typeof(MockServiceOldFeature), oldService);
        }
        
        [Test]
        public void CreateNonMonoServiceDeleteItAndResolveIt()
        {
            Debug.unityLogger.logEnabled = false;
            serviceLocatorManager.Register<IMockInterface>(new MockServiceOldFeature());

            serviceLocatorManager.Reset();

            IMockInterface service = serviceLocatorManager.Resolve<IMockInterface>();
            Assert.IsNull(service);
        }
        
        [Test]
        public void CreateMonoServiceAndResolveIt()
        {
            serviceLocatorManager.Register<IMockInterface>(ServiceLocatorManager.AsMono<MonoMockServiceFeature>());
            
            IMockInterface service = serviceLocatorManager.Resolve<IMockInterface>();
            Assert.IsInstanceOf(typeof(MonoMockServiceFeature), service);
        }
        
        [Test]
        public void CreateMonoServiceResetItAndResolveIt()
        {
            Debug.unityLogger.logEnabled = false;

            serviceLocatorManager.Register<IMockInterface>(ServiceLocatorManager.AsMono<MonoMockServiceFeature>());
            
            serviceLocatorManager.Reset();

            IMockInterface service = serviceLocatorManager.Resolve<IMockInterface>();
            var obj = Object.FindObjectOfType<MonoMockServiceFeature>();
            Assert.IsNull(obj);
            
        }

        [Test]
        public void DefaultErrorLog()
        {
            LogAssert.Expect(LogType.Error, "ServiceLocator - Cant find Service of type: " + nameof(IMockInterface));
            var test = serviceLocatorManager.Resolve<IMockInterface>();
            LogAssert.NoUnexpectedReceived();
            Assert.IsNull(test);
        }
        
        [Test]
        public void NullLoggerService()
        {
            serviceLocatorManager.Register<IServiceLocatorManagerLogger>(null);
            Assert.IsNull(serviceLocatorManager.Resolve<IServiceLocatorManagerLogger>());
            var test = serviceLocatorManager.Resolve<IMockInterface>();
            Assert.IsNull(test);
            serviceLocatorManager.Register<IMockInterface>(new MockServiceNewFeature());
            serviceLocatorManager.Reset();
            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void MockLogger()
        {
            LogAssert.Expect(LogType.Log, ServiceManagerLogType.Registered.ToString());
            LogAssert.Expect(LogType.Log, ServiceManagerLogType.Missing.ToString());
            serviceLocatorManager.Register<IServiceLocatorManagerLogger>(new MockLogger());
            var test = serviceLocatorManager.Resolve<IMockInterface>();
            Assert.IsNull(test);
            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void LogIsDefaultLoggerAfterReset()
        {
            LogAssert.Expect(LogType.Log, ServiceManagerLogType.Registered.ToString());
            LogAssert.Expect(LogType.Log, ServiceManagerLogType.Resetting.ToString());
            serviceLocatorManager.Register<IServiceLocatorManagerLogger>(new MockLogger());
            serviceLocatorManager.Reset();
            LogAssert.Expect(LogType.Error, "ServiceLocator - Cant find Service of type: " + nameof(IMockInterface));
            var test = serviceLocatorManager.Resolve<IMockInterface>();
            LogAssert.NoUnexpectedReceived();
            Assert.IsNull(test);
        }
        
        [Test]
        public void LogRegisterEvent()
        {
            //expect register to be logged twice. Once for the new logger and once for the mock interface.
            LogAssert.Expect(LogType.Log, ServiceManagerLogType.Registered.ToString());
            LogAssert.Expect(LogType.Log, ServiceManagerLogType.Registered.ToString());
            serviceLocatorManager.Register<IServiceLocatorManagerLogger>(new MockLogger());
            serviceLocatorManager.Register<IMockInterface>(new MockServiceNewFeature());
            var test = serviceLocatorManager.Resolve<IMockInterface>();
            LogAssert.NoUnexpectedReceived();
            Assert.IsNotNull(test);
        }
        
        [Test]
        public void LogResettingEvent()
        {
            LogAssert.Expect(LogType.Log, ServiceManagerLogType.Registered.ToString());
            serviceLocatorManager.Register<IServiceLocatorManagerLogger>(new MockLogger());
            LogAssert.Expect(LogType.Log, ServiceManagerLogType.Resetting.ToString());
            serviceLocatorManager.Reset();
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTearDown]
        public IEnumerator AfterEachTest()
        {
            serviceLocatorManager.Reset();
            Debug.unityLogger.logEnabled = true;
            yield break;
        }
    }
}
