using System.Collections;
using System.Collections.Generic;
using GameFrame;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GxhTest
{
    public class testBase
    {
        public testBase()
        {
            Debug.Log("xxx");
            asd();
        }

        public virtual void asd()
        {
        }
    }

    public class testChild : testBase
    {
        public testChild()
        {
        }

        public override void asd()
        {
        }
    }

    [Test]
    public void GxhTestSimplePasses()
    {
        testChild x = new testChild();
    }

    [Test]
    public void StrongList()
    {
        StrongList<int> list = new StrongList<int>(10, true);
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

    [Test]
    public void staticeTest()
    {
        staticClass.x = 5;
    }


    public static class staticClass
    {
        public static int x = 0;

        static staticClass()
        {
            Debug.Log("staticClass");
        }
    }
}