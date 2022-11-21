using System.Collections.Generic;

namespace Euler.Core
{
    internal class ContinuousFraction
    {
        public long Frequency { get { return Digits.Count; } }

        public long IntegerPart { get; internal set; }
        public List<long> Digits { get; internal set; }

        internal ContinuousFraction()
        {
            Digits = new List<long>();
        }
    }
}