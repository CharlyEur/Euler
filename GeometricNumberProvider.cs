
using System;

using System.Collections.Generic;

using System.Linq;



namespace Euler.Core

{

    public enum GeometricForm

    {

        Triangle,

        Square,

        Pentagon,

        Hexagon,

        Heptagon,

        Octogon,

    }



    public class GeometricNumbersProvider

    {

        private readonly List<long> triangles;

        private readonly List<long> squares;

        private readonly List<long> pentagons;

        private readonly List<long> hexagons;

        private readonly List<long> heptagons;

        private readonly List<long> octogons;



        public GeometricNumbersProvider(int sizeMax)

        {

            triangles = new List<long>();

            pentagons = new List<long>();

            hexagons = new List<long>();

            octogons = new List<long>();

            squares = new List<long>();

            heptagons = new List<long>();



            for (int i = 1; i < sizeMax; i++)

            {

                triangles.Add(Triangle(i));

                pentagons.Add(Pentagon(i));

                hexagons.Add(Hexagon(i));

                octogons.Add(Octogon(i));

                squares.Add(Square(i));

                heptagons.Add(Heptagon(i));

            }

        }



        #region Functions



        private static long Triangle(long candidate) { return candidate * (candidate + 1) / 2; }

        private static long Square(long candidate) { return candidate * candidate; }

        private static long Pentagon(long candidate) { return candidate * (3 * candidate - 1) / 2; }

        private static long Hexagon(long candidate) { return candidate * (2 * candidate - 1); }

        private static long Heptagon(long candidate) { return candidate * (5 * candidate - 3) / 2; }

        private static long Octogon(long candidate) { return candidate * (3 * candidate - 2); }



        #endregion



        #region Checkers



        public static bool IsTriangle(long candidate)

        {

            double testpart = Math.Sqrt(1 + 8 * candidate);

            return testpart.IsInteger() && (int)testpart % 2 == 1;

        }



        public static bool IsSquare(long candidate)

        {

            double testpart = Math.Sqrt(candidate);

            return testpart.IsInteger();

        }



        public static bool IsPentagon(long candidate)

        {

            double testpart = Math.Sqrt(1 + 24 * candidate);

            return testpart.IsInteger() && (int)testpart % 6 == 5;

        }



        public static bool IsHexagon(long candidate)

        {

            double testpart = Math.Sqrt(1 + 8 * candidate);

            return testpart.IsInteger() && (int)testpart % 4 == 3;

        }



        public static bool IsHeptagon(long candidate)

        {

            double testpart = Math.Sqrt(9 + 40 * candidate);

            return testpart.IsInteger() && (int)testpart % 10 == 7;

        }



        public static bool IsOctogon(long candidate)

        {

            double testpart = Math.Sqrt(4 + 12 * candidate);

            return testpart.IsInteger() && (int)testpart % 6 == 4;

        }



        #endregion



        #region Enumerables



        public IEnumerable<long> Triangles

        {

            get { return triangles; }

        }



        public IEnumerable<long> Squares

        {

            get { return squares; }

        }



        public IEnumerable<long> Pentagons

        {

            get { return pentagons; }

        }



        public IEnumerable<long> Hexagons

        {

            get { return hexagons; }

        }



        public IEnumerable<long> Heptagons

        {

            get { return heptagons; }

        }



        public IEnumerable<long> Octogons

        {

            get { return octogons; }

        }



        #endregion



        internal static long FindPentagonHypothesis(int maxIdx)

        {

            var provider = new GeometricNumbersProvider(maxIdx);



            var threshold = (double)maxIdx * 2 / 3;



            var pool = new List<long>();



            for (var i = 0; i < threshold; i++)

            {

                for (var j = 0; j < i; j++)

                {

                    var pentaSum = provider.pentagons[i] + provider.pentagons[j];



                    if (!IsPentagon(pentaSum))

                        continue;



                    long pentaDiff = provider.pentagons[i] - provider.pentagons[j];



                    if (IsPentagon(pentaDiff))

                        pool.Add(pentaDiff);

                }

            }



            return pool.Min();

        }



        internal static long FindTriPentaHexa(int triangleStart)

        {

            var provider = new GeometricNumbersProvider(100000);

            int index = triangleStart;



            var trianglePool = provider.Triangles.ToList();



            while (true)

            {

                var candidate = trianglePool[index];



                if (IsPentagon(candidate) && IsHexagon(candidate))

                    return candidate;



                index++;

            }

        }



        internal static long FindMagicCycle()

        {

            var provider = new GeometricNumbersProvider(150); // all triangles above 10000 are in



            foreach (var octogon in provider.Octogons.Where(x => x >= 1000 && x < 10000))

            {

                var cycle

                    = new List<Tuple<GeometricForm, List<short>>>

                    {

                        new Tuple<GeometricForm, List<short>>(GeometricForm.Octogon, Decomposition.Decompose(octogon, 10, 4))

                    };



                if (provider.FindPath(cycle))

                    return cycle.Select(x => Decomposition.Recompose(x.Item2, 10)).Sum();

            }



            return 0;

        }



        internal bool FindPath(List<Tuple<GeometricForm, List<short>>> cycle)

        {

            var lastItem = cycle[cycle.Count - 1].Item2;



            if (cycle.Count == 5)

            {

                var firstItem = cycle[0].Item2;



                var type = FindRemainingTypes(cycle)[0];



                var candidateDigits = new List<short> { lastItem[2], lastItem[3], firstItem[0], firstItem[1] };

                var candidate = Decomposition.Recompose(candidateDigits, 10);



                if (!GetFunction(type)(candidate))

                    return false;



                cycle.Add(new Tuple<GeometricForm, List<short>>(type, candidateDigits));

                return true;

            }



            var remainingTypes = FindRemainingTypes(cycle);

            var lowerBoundDigits = new List<short> { lastItem[2], lastItem[3], 0, 0 };

            var lowerBound = Decomposition.Recompose(lowerBoundDigits, 10);



            foreach (var geometricForm in remainingTypes)

            {

                List<long> candidates;

                if (SearchTypeAmong(geometricForm, lowerBound, 100, out candidates))

                {

                    for (var i = 0; i < candidates.Count; i++)

                    {

                        cycle.Add(new Tuple<GeometricForm, List<short>>(geometricForm, Decomposition.Decompose(candidates[i], 10, 4)));



                        if (!FindPath(cycle))

                            cycle.RemoveAt(cycle.Count - 1);

                        else

                            return true;

                    }

                }

            }



            return false;

        }



        private static List<GeometricForm> FindRemainingTypes(List<Tuple<GeometricForm, List<short>>> cycle)

        {

            var forms = Enum.GetNames(typeof(GeometricForm)).Select(x => (GeometricForm)Enum.Parse(typeof(GeometricForm), x)).ToList();



            foreach (Tuple<GeometricForm, List<short>> t in cycle)

                forms.Remove(t.Item1);



            return forms;

        }



        internal bool SearchTypeAmong(GeometricForm type, long start, long shift, out List<long> results)

        {

            results = GetCollection(type).Where(x => x >= start && x < (start + shift)).ToList();



            if (start < 1000)

                return false;



            return results.Any();

        }



        private static Func<long, bool> GetFunction(GeometricForm type)

        {

            switch (type)

            {

                case GeometricForm.Triangle:

                    return IsTriangle;

                case GeometricForm.Square:

                    return IsSquare;

                case GeometricForm.Pentagon:

                    return IsPentagon;

                case GeometricForm.Hexagon:

                    return IsHexagon;

                case GeometricForm.Heptagon:

                    return IsHeptagon;

                case GeometricForm.Octogon:

                    return IsOctogon;



                default:

                    throw new NotImplementedException("Unhandled enum type");

            }

        }



        private IEnumerable<long> GetCollection(GeometricForm type)

        {

            switch (type)

            {

                case GeometricForm.Triangle:

                    return Triangles;

                case GeometricForm.Square:

                    return Squares;

                case GeometricForm.Pentagon:

                    return Pentagons;

                case GeometricForm.Hexagon:

                    return Hexagons;

                case GeometricForm.Heptagon:

                    return Heptagons;

                case GeometricForm.Octogon:

                    return Octogons;



                default:

                    throw new NotImplementedException("Unhandled enum type");

            }

        }

    }

}