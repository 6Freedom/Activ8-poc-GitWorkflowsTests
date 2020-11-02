using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class BasicTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void BasicTestsSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator BasicTestsWithEnumeratorPasses()
        {
            var camera = GameObject.Instantiate(Resources.Load<GameObject>("Main Camera"));

            Assert.IsNotNull(camera.GetComponent<Shoot>().Sphere);
            
            yield return null;
        }
    }
}
