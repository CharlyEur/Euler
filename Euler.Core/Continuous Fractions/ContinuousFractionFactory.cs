using System;
using System.Collections.Generic;

namespace Euler.Core
{
    internal class ContinuousFractionsFactory
    {
        private const int Digit_Limit = 1000;

        public static ContinuousFraction SimpleCreate(double input)
        {
            long integerPart = (long)Math.Floor(input);
            var digits = CreateDigits(input - integerPart);
            return new ContinuousFraction { IntegerPart = integerPart, Digits = digits };
        }

        private static List<long> CreateDigits(double remainder)
        {
            if (remainder > 1)
                throw new ArgumentOutOfRangeException();

            var digits = new List<long>();

            if (remainder < double.Epsilon)
                return digits;

            var firstRemainder = remainder;
            var currentRemainder = remainder;

            while (digits.Count < Digit_Limit)
            {
                var candidate = 1 / currentRemainder;
                var candidateDigit = (long)Math.Floor(candidate);
                currentRemainder = candidate - candidateDigit;
                digits.Add(candidateDigit);

                if (Math.Abs(firstRemainder - currentRemainder) < Math.Pow(10, -15))
                    return digits;
            }

            return digits;
        }

        public static ContinuousFraction AdvancedRootCreate(long square)
        {
            var digits = new List<long>();

            if (GeometricNumbersProvider.IsSquare(square))
                return new ContinuousFraction { IntegerPart = (long)Math.Sqrt(square), Digits = digits };

            var toUse = new RootAtom { SquareCandidate = square, NumeratorInteger = 0, Denominator = 1 };
            var buffer = toUse.Reduce();
            var integerPart = buffer.Item1;

            var i = 0;
            var existingAtoms = new HashSet<RootAtom>();

            while (!existingAtoms.Contains(buffer.Item2) && i < Digit_Limit)
            {
                existingAtoms.Add(buffer.Item2);

                buffer = buffer.Item2.Reduce();
                digits.Add(buffer.Item1);

                i++;
            }

            return new ContinuousFraction { IntegerPart = integerPart, Digits = digits };
        }

        internal class RootAtom : IEquatable<RootAtom>
        {
            public long SquareCandidate { get; set; }

            public long NumeratorInteger { get; set; }

            public long Denominator { get; set; }

            public Tuple<long, RootAtom> Reduce()
            {
                var integerPart = (long)((Math.Sqrt(SquareCandidate) + NumeratorInteger) / Denominator);

                var newNum = NumeratorInteger - Denominator * integerPart;
                var opposite = -newNum;
                var newDenom = (SquareCandidate - opposite * opposite) / Denominator;

                return new Tuple<long, RootAtom>(integerPart, new RootAtom
                {
                    SquareCandidate = SquareCandidate,
                    NumeratorInteger = opposite,
                    Denominator = newDenom
                });
            }

            public bool Equals(RootAtom other)
            {
                if (ReferenceEquals(null, other))
                    return false;

                if (ReferenceEquals(this, other))
                    return true;

                return SquareCandidate == other.SquareCandidate
                    && NumeratorInteger == other.NumeratorInteger
                    && Denominator == other.Denominator;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                    return false;

                if (ReferenceEquals(this, obj))
                    return true;

                if (obj.GetType() != this.GetType())
                    return false;

                return Equals((RootAtom)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = SquareCandidate.GetHashCode();
                    hashCode = (hashCode * 397) ^ NumeratorInteger.GetHashCode();
                    hashCode = (hashCode * 397) ^ Denominator.GetHashCode();
                    return hashCode;
                }
            }

            public static bool operator ==(RootAtom left, RootAtom right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(RootAtom left, RootAtom right)
            {
                return !Equals(left, right);
            }
        }
    }
}