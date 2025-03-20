using System;
using System.Collections.Generic;
using GameFrame;

namespace GXGame
{
    public class MinHeap<T>
    {
        private List<T> heap;

        public int Count => heap.Count;

        private Func<T, T, int> compareTo;

        public T this[int index]
        {
            get
            {
                Assert.IsTrue(index < heap.Count, "数组越界");
                return heap[index];
            }
        }

        public MinHeap(int capacity, Func<T, T, int> compareTo)
        {
            heap = new List<T>(capacity);
            this.compareTo = compareTo;
        }

        public void Insert(T value)
        {
            heap.Add(value);
            BubbleUp(heap.Count - 1);
        }

        public T GetMin()
        {
            if (heap.Count == 0)
                return default;
            return heap[0];
        }

        public T DeleteMin()
        {
            if (heap.Count == 0)
                throw new InvalidOperationException("Heap is empty");

            T min = heap[0];
            heap[0] = heap[^1];
            heap.RemoveAt(heap.Count - 1);
            BubbleDown(0);
            return min;
        }

        public void Update(T t)
        {
            int index = heap.IndexOf(t);
            Swap(index, Count - 1);
            BubbleUp(Count - 1);
        }

        private void BubbleUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (compareTo(heap[index], heap[parent]) < 0)
                {
                    Swap(index, parent);
                    index = parent;
                }
                else break;
            }
        }

        private void BubbleDown(int index)
        {
            int last = heap.Count - 1;
            while (true)
            {
                int leftChild = 2 * index + 1;
                int rightChild = 2 * index + 2;
                int smallest = index;

                if (leftChild <= last && compareTo(heap[leftChild], heap[smallest]) < 0)
                    smallest = leftChild;

                if (rightChild <= last && compareTo(heap[rightChild], (heap[smallest])) < 0)
                    smallest = rightChild;

                if (smallest != index)
                {
                    Swap(index, smallest);
                    index = smallest;
                }
                else break;
            }
        }

        public void Clear()
        {
            heap.Clear();
        }

        private void Swap(int i, int j)
        {
            (heap[i], heap[j]) = (heap[j], heap[i]);
        }

        public IEnumerator<T> GetEnumerator() => this.heap.GetEnumerator();
    }
}