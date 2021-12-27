using System;
using Random = System.Random;

namespace SDM.Utils
{
    public class Utils
    {
        
        /// <summary>
        /// https://www.alanzucconi.com/2015/09/16/how-to-sample-from-a-gaussian-distribution/
        /// See also https://numerics.mathdotnet.com/
        /// </summary>
        internal static double SampleGaussian(Random random, double mean, double std)
        {
            double u, v, s;
            do
            {
                u = random.NextDouble() * 2.0 - 1.0;
                v = random.NextDouble() * 2.0 - 1.0;
                s = u * u + v * v;
            }
            while (s >= 1.0 || Math.Abs(s) < double.Epsilon);

            s = Math.Sqrt(-2.0 * Math.Log(s) / s);

            return v * s * std + mean;
        }
    }
}