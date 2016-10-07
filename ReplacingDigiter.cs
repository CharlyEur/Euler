using System.Collections.Generic;
using System.Linq;


namespace Euler.Core

{
    public class ReplacingDigiter
    {
        public List<short> Digits { get; set; }

        public List<int> ReplacedIndex { get; set; }

        public IEnumerable<long> Family { get { return GenerateFamily(); } }

        public ReplacingDigiter()
        {
        }
        
        private IEnumerable<long> GenerateFamily()
        {
            var copy = Digits.ToArray();
            
            for (short i = 0; i < 10; i++)
            {
                foreach (var idx in ReplacedIndex)
                    copy[idx] = i;

                yield return Decomposition.Recompose(copy.ToList(), 10);
            }
        }
    }
}