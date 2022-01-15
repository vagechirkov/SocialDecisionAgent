using System;
using Random = UnityEngine.Random;

namespace SDM.Utils
{
    public class Utils
    {
        
        /// <summary>
        /// https://www.alanzucconi.com/2015/09/16/how-to-sample-from-a-gaussian-distribution/
        /// See also https://numerics.mathdotnet.com/
        /// </summary>
        internal static float SampleGaussian(float mean, float std)
        {
            float u, v, s;
            do
            {
                u = Random.value * 2.0f - 1.0f;
                v = Random.value * 2.0f - 1.0f;
                s = u * u + v * v;
            }
            while (s >= 1.0f || Math.Abs(s) < float.Epsilon);

            s = (float) Math.Sqrt(-2.0f * Math.Log(s) / s);

            return v * s * std + mean;
        }
    }
}