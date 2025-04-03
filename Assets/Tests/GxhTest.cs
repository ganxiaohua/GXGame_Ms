using System.Collections;
using System.Collections.Generic;
using GameFrame;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GxhTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void GxhTestSimplePasses()
    {
        // Use the Assert class to test conditions
    }
    
    [Test]
    public void StrongList()
    {
        StrongList<int> list = new StrongList<int>(10,true);
        list.Add(1);
        list.Add(2);
        list.Add(3);
        list.Add(4);
        list.Add(5);
        foreach (var sk in list)
        {
            if (sk == 3)
            {
                list.Remove(1);
            }
            Debugger.Log(sk);
        }
        
        foreach (var sk in list)
        {
            Debugger.Log(sk);
        }
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator GxhTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
