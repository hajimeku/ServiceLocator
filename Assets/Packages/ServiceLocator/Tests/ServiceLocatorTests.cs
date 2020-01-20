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
            ServiceLocatorManager.Register<IMockInterface>(ServiceLocatorManager.AsMono<MonoMockServiceFeature>());
            
            IMockInterface service = ServiceLocatorManager.Resolve<IMockInterface>();
            Assert.IsInstanceOf(typeof(MonoMockServiceFeature), service);
        }
        
        [UnityTest]
        public IEnumerator CreateMonoServiceResetItAndResolveIt()
        {
            Debug.unityLogger.logEnabled = false;

            ServiceLocatorManager.Register<IMockInterface>(ServiceLocatorManager.AsMono<MonoMockServiceFeature>());
            
            ServiceLocatorManager.Reset();

            IMockInterface service = ServiceLocatorManager.Resolve<IMockInterface>();
            Assert.IsNull(service);
            yield return new WaitForEndOfFrame();
            var obj = Object.FindObjectOfType<MonoMockServiceFeature>();
            Assert.IsNull(obj);
        }

        [UnitySetUp]
        public IEnumerator AfterEachTest()
        {
            Debug.unityLogger.logEnabled = true;
            yield break;
        }
    }
}
