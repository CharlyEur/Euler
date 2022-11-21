

using System;

using System.Collections.Generic;

using System.Linq;



namespace Euler.Core

{

    public class GaussianCribleBuilder
    {
        internal PrimalityProvider PrimeCrible { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        /// on a que le huitième de plan supérieur collé aux abcisses à parcourir</remarks>
        /// <returns></returns>
        internal List<GaussianInteger> BuildCrible()
        {

            int moduleSup = (int)Math.Sqrt(PrimeCrible.MaxSize);
            bool[,] nonPrimeMatrix = new bool[moduleSup, moduleSup];

            var tempCrible = new List<GaussianInteger>();

            AddToCribleRaw(GaussianInteger.N(1, 1), tempCrible, nonPrimeMatrix, moduleSup, true);

            for (var moduleMax = 2; moduleMax < moduleSup; moduleMax++) // déplacement dans la "bande" i <= m < i+1
                GoThroughZone(moduleMax, tempCrible, nonPrimeMatrix, moduleSup);

            return tempCrible;
        }



        private static void AddToCrible(GaussianInteger gaussianInteger, List<GaussianInteger> tempCrible, bool[,] nonPrimeMatrix, long sizeMax, bool isLowerPart)
        {
            tempCrible.Add(gaussianInteger);

            foreach (var point in PathEngine.GenerateHalfCirclePath(1).Where(x => x.Module > 1))
            {
                var candidate = point * gaussianInteger;

                if (candidate.IsInUpperRightSquare(sizeMax))
                    nonPrimeMatrix[candidate.A, candidate.B] = true;
            }

            var startingModule = 2;
            while (startingModule * gaussianInteger.Module < sizeMax)
            {
                foreach (var point in PathEngine.GenerateHalfCirclePath(startingModule, isLowerPart)
                                                .Select(x => x * gaussianInteger)
                                                .Where(point => point.IsInUpperRightSquare(sizeMax)))
                {
                    nonPrimeMatrix[point.A, point.B] = true;
                }

                startingModule++;
            }
        }
        
        private static void AddToCribleRaw(GaussianInteger gaussianInteger, List<GaussianInteger> tempCrible, bool[,] nonPrimeMatrix, int sizeMax, bool isLowerPart)

        {

            tempCrible.Add(gaussianInteger);

            var maxModule = Math.Ceiling(sizeMax / gaussianInteger.Module);



            foreach (var point in PathEngine.GenerateRawPath(maxModule, gaussianInteger, sizeMax))

                nonPrimeMatrix[point.A, point.B] = true;

        }



        private void GoThroughZone(long module, List<GaussianInteger> tempCrible, bool[,] nonPrimeMatrix, int sizeMax)

        {

            var tryPoint = (int)Math.Ceiling(Math.Cos(Math.PI / 4) * module);

            var points = PathEngine.GenerateModuleWisePath(GaussianInteger.N(tryPoint, tryPoint), module);



            foreach (var point in points)

                CheckCrible(tempCrible, point, nonPrimeMatrix, sizeMax);

        }



        private void CheckCrible(List<GaussianInteger> tempCrible, GaussianInteger candidate, bool[,] nonPrimeMatrix, int sizeMax)

        {

            if (nonPrimeMatrix[candidate.A, candidate.B])

                return;



            if (!Challenge(candidate))

                return;



            AddToCribleRaw(candidate, tempCrible, nonPrimeMatrix, sizeMax, true);



            if (candidate.B > 0)

                AddToCribleRaw(candidate.Friend, tempCrible, nonPrimeMatrix, sizeMax, false);

        }



        /// <summary>

        /// Used to be used, but too time-consuming.

        /// </summary>

        /// <param name="tempCrible"></param>

        /// <param name="candidate"></param>

        /// <returns></returns>

        private static bool FindDivisor(List<GaussianInteger> tempCrible, GaussianInteger candidate)

        {

            foreach (var divisor in tempCrible)

            {

                if (divisor.Divide(candidate))

                    return true;

            }



            return false;

        }



        private bool Challenge(GaussianInteger candidate)

        {

            long a = candidate.A;

            long b = candidate.B;



            if (a == b)

                return false;



            double borne = Math.Min(a, b);



            int cursor = 0;

            long primeChosen = PrimeCrible[cursor];



            while (primeChosen < borne && cursor < PrimeCrible.Count)

            {

                primeChosen = PrimeCrible[cursor];



                if (a % primeChosen == 0 && b % primeChosen == 0)

                    return false;



                cursor++;

            }



            return true;

        }



        internal List<GaussianInteger> OtherBuildCrible()

        {

            var tempCrible = new List<GaussianInteger>();



            tempCrible.Add(GaussianInteger.N(1, 1));



            for (var i = 1; i < PrimeCrible.Count; i++)

            {

                Tuple<long, long> squareDecompose;



                if (TryToDecompose(PrimeCrible[i], out squareDecompose))

                {

                    tempCrible.Add(GaussianInteger.N(squareDecompose.Item1, squareDecompose.Item2));

                    tempCrible.Add(GaussianInteger.N(squareDecompose.Item2, squareDecompose.Item1));

                }

                else if (PrimeCrible[i] < Math.Sqrt(PrimeCrible.MaxSize))

                    tempCrible.Add(GaussianInteger.N(PrimeCrible[i], 0));

            }



            return tempCrible;

        }



        internal List<double> BuildCribleModules()

        {

            var modules = new List<double>();



            modules.Add(Math.Sqrt(2));



            for (long i = 1; i < PrimeCrible.Count; i++)

            {

                if (PrimeCrible[(int)i] % 4 == 1)

                {

                    modules.Add(Math.Sqrt(PrimeCrible[(int)i]));

                    modules.Add(Math.Sqrt(PrimeCrible[(int)i]));

                }

                else if (PrimeCrible[(int)i] < Math.Sqrt(PrimeCrible.MaxSize))

                    modules.Add(PrimeCrible[(int)i]);

            }



            return modules;

        }



        private static bool TryToDecompose(long primeCandidate, out Tuple<long, long> squareDecomposition)

        {

            squareDecomposition = new Tuple<long, long>(0, 0);



            if ((primeCandidate % 4) != 1)

                return false;



            for (int i = 1; i < Math.Sqrt(primeCandidate); i++)

            {

                var jSquare = primeCandidate - i * i;



                double jSquareRoot = Math.Sqrt(jSquare);



                if (jSquareRoot.IsInteger())

                {

                    squareDecomposition = new Tuple<long, long>(i, (int)jSquareRoot);

                    return true;

                }

            }

            return false;

        }

    }

}