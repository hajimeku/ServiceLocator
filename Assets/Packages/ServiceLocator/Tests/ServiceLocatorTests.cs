using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using ServiceLocator;
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

        [UnityTearDown]
        public IEnumerator AfterEachTest()
        {
            serviceLocatorManager.Reset();
            Debug.unityLogger.logEnabled = true;
            yield break;
        }
    }
}
