using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler.Core
{
    public class Fraction
    {
        public long Numerator { get; private set; }
        public long Denominator { get; private set; }
        
        private Fraction()
        {
            Numerator = 0;
            Denominator = 1;
        }

        internal Fraction(long num, long denom)
        {
            Numerator = num;
            Denominator = denom;
        }

        public override string ToString()
        {
            return string.Format("{0}/{1} ~ {2}", Numerator, Denominator, (double)Numerator / Denominator);
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != this.GetType())
                return false;

            return Equals((Fraction)obj);
        }

        public bool Equals(Fraction other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Numerator == other.Numerator
                && Denominator == other.Denominator;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Numerator.GetHashCode();
                hashCode = (hashCode * 397) ^ Denominator.GetHashCode();
                return hashCode;
            }
        }
    }
}
