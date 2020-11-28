using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrypterCore
{
    public struct CyclicArray<T>
    {
        private readonly T[] data;

        public int Length => data.Length;

        public T this[int index]
        {
            get => index >= 0 
                ? data[index % data.Length] 
                : data[data.Length - (-index % data.Length)];
            set
            {
                if (index >= 0)
                {
                    data[index % data.Length] = value;
                }
                else
                {
                    data[data.Length - (-index % data.Length)] = value;
                }
            }
        }

        public CyclicArray(int length)
        {
            data = new T[length];
        }

        public CyclicArray(T[] source)
        {
            data = source;
        }

        public CyclicArray(int length, Func<int, T> generator) : this(length)
        {
            for (var i = 0; i < length; ++i)
            {
                data[i] = generator(i);
            }
        }

        public CyclicArray(int length, IReadOnlyList<T> fill) : this(length, (i) => fill[i % fill.Count])
        {
        }

        public T[] ToArray()
        {
            return data.ToArray();
        }
    }
}