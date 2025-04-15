using System.Collections.Generic;
using System.Diagnostics;
using GameFrame;
using NUnit.Framework;
using Debug = UnityEngine.Debug;

public partial class GxhTest
{
    public class ContinuousClass : IContinuousID
    {
        public int ID { get; set; }
    }

    [Test]
    public void GXHash()
    {
        List<ContinuousClass> tt = new();
        for (int i = 0; i < 10000; i++)
        {
            tt.Add(new ContinuousClass() {ID = i});
        }

        HashSet<ContinuousClass> temp2 = new HashSet<ContinuousClass>(tt.Count);
        var sw = Stopwatch.StartNew();
        foreach (var ss in tt)
        {
            temp2.Add(ss);
        }

        foreach (var ss in tt)
        {
            temp2.Remove(ss);
        }

        sw.Stop();
        Debug.Log(sw.Elapsed.TotalMilliseconds);

        GXHashSet<ContinuousClass> temp = new GXHashSet<ContinuousClass>(tt.Count);
        sw = Stopwatch.StartNew();
        foreach (var ss in tt)
        {
            temp.Add(ss);
        }

        foreach (var ss in tt)
        {
            temp.Remove(ss);
        }

        sw.Stop();
        Debug.Log(sw.Elapsed.TotalMilliseconds);
    }

    [Test]
    public void GXList()
    {
    }
}