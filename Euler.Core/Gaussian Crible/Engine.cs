using System;
using System.Collections.Generic;
using System.Linq;
using GInt = Euler.Core.GaussianInteger;

namespace Euler.Core
{
    public class Engine
    {
        private readonly PrimalityProvider provider;
        private readonly List<GInt> crible;
        private readonly GaussianCribleBuilder builder;

        public Engine(int sizeMax)
        {
            provider = new PrimalityProvider(sizeMax);
            builder = new GaussianCribleBuilder { PrimeCrible = provider };
            crible = builder.OtherBuildCrible();
            //crible = builder.BuildCrible();
        }

        public Tuple<int, int> Bornes
        {
            get
            {
                int borneSup = (int) Math.Ceiling(Math.Sqrt(provider.MaxSize));
                int borneInf = -borneSup + 1;

                return new Tuple<int, int>(borneInf, borneSup);
            }
        }

        /// <summary>
        /// Compter toutes les combinaisons de premiers de module inférieur à sizeMax
        /// </summary>
        /// <returns></returns>
        public int ComputeCount()
        {
            double limit = Math.Sqrt(provider.MaxSize);
            int count = 1; // to include 1
            List<double> moduleValues = crible.Where(x => x.Module < limit).Select(x => x.Module).ToList();
            moduleValues.Sort();
            moduleValues.Reverse();

            count += AnalyseProductsIterative(moduleValues.ToArray(), Math.Sqrt(provider.MaxSize));

            return count;
        }

        internal IList<double> BuildModules()
        {
            return builder.BuildCribleModules();
        }

        public int AlternateCount()
        {
            List<double> moduleValues = builder.BuildCribleModules();
            moduleValues.Sort();
            moduleValues.Reverse();

            int count = 1;

            count += AnalyseProductsIterative(moduleValues.ToArray(), Math.Sqrt(provider.MaxSize));

            return count;
        }

        internal static int RecursiveAnalyseProducts(double[] moduleArray, double limit)
        {
            return ParseModuleStack(moduleArray, 0, limit);
        }

        private static int ParseModuleStack(double[] modules, int start, double limit)
        {
            if (start == modules.Length)
                return 0;

            var candidate = modules[start];

            int local = (candidate <= limit)
                ? 1
                : 0;

            int notUsingPop = ParseModuleStack(modules, start + 1, limit);

            if (limit / candidate < Math.Sqrt(2))
                return local + notUsingPop;

            int usingPop = ParseModuleStack(modules, start + 1, limit / candidate);

            return local + usingPop + notUsingPop;
        }

        internal static int AnalyseProductsIterative(double[] moduleArray, double limit)
        {
            int totalCount = 0;
            Tuple<int, double> startingPoint = new Tuple<int, double>(0, limit);

            var solvingStack = new Stack<Tuple<int, double>>();

            solvingStack.Push(startingPoint);

            while (solvingStack.Count > 0)
            {
                var pbData = solvingStack.Pop();

                if (pbData.Item1 == moduleArray.Length)
                    continue;

                var candidate = moduleArray[pbData.Item1];

                if (candidate <= pbData.Item2)
                    totalCount++;

                solvingStack.Push(new Tuple<int, double>(pbData.Item1 + 1, pbData.Item2));

                if (pbData.Item2 / candidate >= Math.Sqrt(2))
                    solvingStack.Push(new Tuple<int, double>(pbData.Item1 + 1, pbData.Item2 / candidate));
            }

            return totalCount;
        }

        public int CribleSize
        {
            get { return crible.Count; }
        }
    }
}