using System;
using System.Collections.Generic;

namespace Euler.Core
{
    internal static class Toolkit
    {
        public class Comparer<T> : IEqualityComparer<T>
        {
            private Func<T, T, bool> _comparer;
            private Func<T, int> _hashCoder;

            public Comparer( Func<T,T,bool> comparer, Func<T, int> hashCodeGenerator )
            {
                _comparer = comparer;
                _hashCoder = hashCodeGenerator;
            }

            bool IEqualityComparer<T>.Equals(T x, T y)
            {
                return _comparer(x, y);
            }

            int IEqualityComparer<T>.GetHashCode(T obj)
            {
                return _hashCoder(obj);
            }
        }
    }
}
