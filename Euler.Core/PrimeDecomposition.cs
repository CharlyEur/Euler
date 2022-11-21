using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Euler.Core
{
    [DebuggerDisplay("{ToString(),nq}")]
    public class PrimeDecomposition : SortedDictionary<long, long>, IEquatable<PrimeDecomposition>
    {
        public override bool Equals(object obj)
            => !(obj is null)
            && (ReferenceEquals(this, obj)
                || obj is PrimeDecomposition other
                    && Equals(other));

        public override int GetHashCode()
        {
            var stringVersion = ToString();

            return stringVersion.GetHashCode();
        }

        public bool Equals(PrimeDecomposition other)
        {
            if (other.Count != Count)
            {
                return false;
            }

            foreach (var key in Keys)
            {
                if (other.Keys.Contains(key))
                {
                    if (other[key] != this[key])
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString() => string.Join(" x ", this.Select(x => $"{x.Key}^{x.Value}"));
    }

    public static class PrimeExtensions
    {
        public static PrimeDecomposition ToPrimeDecomposition(this Dictionary<long, long> factors)
        {
            var result = new PrimeDecomposition();

            foreach (var p in factors)
            {
                result.Add(p.Key, p.Value);
            }

            return result;
        }
    }
}
