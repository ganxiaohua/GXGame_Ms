using System.Collections;
using System.Collections.Generic;

public partial class GxhTest
{
    public class ietms : IEnumerable<int>, IEnumerator<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            Reset();
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            // TODO 在此释放托管资源
        }

        public bool MoveNext()
        {
            throw new System.NotImplementedException();
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }

        public int Current { get; }

        object IEnumerator.Current => Current;
    }
}