using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Text;

namespace Euler.Core.UnitTests
{
    [TestFixture]
    public class PerfTests
    {
        [Test, Ignore("One shot test, confirm string is better than maths :)")]
        public void Decomposition_Test()
        {
            int count = 1000000;
            var localGenerator = new Random();

            var original = new TimeSpan();
            var challenger = new TimeSpan();

            for (int i = 0; i < count; i++)
            {
                var originalTime = new TimeSpan();
                var challengerTime = new TimeSpan();

                var guineaPig = localGenerator.Next(1000, 1000000000);

                var result = TimeControlled(guineaPig, Decomposition.Decompose, out originalTime);
                var resultCandidate = TimeControlled(guineaPig, x => Decomposition.DecomposeRaw(x), out challengerTime);

                CollectionAssert.AreEqual(result, resultCandidate);

                original += originalTime;
                challenger += challengerTime;
            }

            var message = new StringBuilder();

            message.AppendLine("Used dcompo = " + original);
            message.AppendLine("Challenger dcompo = " + challenger);
            message.AppendLine("Used average = " + original.TotalMilliseconds / count);
            message.AppendLine("Challenger average = " + challenger.TotalMilliseconds / count);

            Assert.IsTrue(challenger > original, message.ToString());
            //Assert.IsTrue(false, message.ToString());
        }

        private TResult TimeControlled<TInput, TResult>( TInput input, Func<TInput, TResult> methodToRun, out TimeSpan timeSpanned)
        {
            var chrono = new Stopwatch();

            chrono.Start();
            TResult result = methodToRun(input);
            chrono.Stop();

            timeSpanned = chrono.Elapsed;

            return result;
        }
    }
}
