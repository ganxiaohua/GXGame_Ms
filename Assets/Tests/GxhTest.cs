using NUnit.Framework;
using UnityEngine;

public partial class GxhTest
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
        // var Children = new HashSet<int>(1000);
        // Children.EnsureCapacity(100000);
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