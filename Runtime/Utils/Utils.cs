using System;
using System.IO;
using System.Text;
using Random = UnityEngine.Random;

namespace SocialDecisionAgent.Runtime.Utils
{
    public static class Utils
    {
        
        /// <summary>
        /// https://www.alanzucconi.com/2015/09/16/how-to-sample-from-a-gaussian-distribution/
        /// See also https://numerics.mathdotnet.com/
        /// </summary>
        internal static float SampleGaussian(float mean, float std)
        {
            float v, s;
            do
            {
                var u = Random.value * 2.0f - 1.0f;
                v = Random.value * 2.0f - 1.0f;
                s = u * u + v * v;
            }
            while (s >= 1.0f || Math.Abs(s) < float.Epsilon);

            s = (float) Math.Sqrt(-2.0f * Math.Log(s) / s);

            return v * s * std + mean;
        }

        internal static string[][] ReadCsvFile(string scvPath)
        {
            
            if (File.Exists(scvPath))
            {

                string[] lines = File.ReadAllLines(scvPath, Encoding.Default);

                string[][] result = new string[lines.Length][];

                //Split all lines with a ','
                for (int i = 0; i < lines.Length; i++)
                {
                    result[i] = lines[i].Split(',');
                }
                return result;
            }
            else
            {
                throw new FileNotFoundException();
            }  
        }
    }
}