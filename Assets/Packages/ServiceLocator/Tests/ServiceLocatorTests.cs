using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests
{
    public class ServiceLocatorTests
    {
        private MonoMockServiceFeature _mockMonoService;

        [Test]
        public void CreateNonMonoServiceAndResolveIt()
        {
            ServiceLocatorManager.Register<IMockInterface>(new MockServiceOldFeature());

            IMockInterface oldService = ServiceLocatorManager.Resolve<IMockInterface>();
            Assert.IsInstanceOf(typeof(MockServiceOldFeature), oldService);
        }
        
        [Test]
        public void CreateNonMonoServiceDeleteItAndResolveIt()
        {
            Debug.unityLogger.logEnabled = false;
            ServiceLocatorManager.Register<IMockInterface>(new MockServiceOldFeature());

            ServiceLocatorManager.Reset();

            IMockInterface service = ServiceLocatorManager.Resolve<IMockInterface>();
            Assert.IsNull(service);
        }
        
        [Test]
        public void CreateMonoServiceAndResolveIt()
        {
            ServiceLocatorManager.Register<IMockInterface>(_mockMonoService);
            
            IMockInterface service = ServiceLocatorManager.Resolve<IMockInterface>();
            Assert.IsInstanceOf(typeof(MonoMockServiceFeature), service);
        }
        
        [UnityTest]
        public IEnumerator CreateMonoServiceResetItAndResolveIt()
        {
            
            ServiceLocatorManager.Register<IMockInterface>(_mockMonoService);
            ServiceLocatorManager.Reset();

            IMockInterface service = ServiceLocatorManager.Resolve<IMockInterface>();
            Assert.IsNull(service);
            yield return null;
            Assert.IsTrue(_mockMonoService == null);
        }

        [UnitySetUp]
        public IEnumerator BeforeEachTest()
        {
            Debug.unityLogger.logEnabled = false;
            _mockMonoService = ServiceLocatorManager.AsMono<MonoMockServiceFeature>();
            yield break;
        }

        [UnityTearDown]
        public IEnumerator AfterEachTest()
        {
            ServiceLocatorManager.Reset();
            if (_mockMonoService) Object.Destroy(_mockMonoService);
            Debug.unityLogger.logEnabled = true;
            yield break;
        }
    }
}
