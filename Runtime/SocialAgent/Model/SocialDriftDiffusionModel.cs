using UnityEngine;

namespace SocialDecisionAgent.Runtime.SocialAgent.Model
{
    public class SocialDriftDiffusionModel: IAgentModel
    {
        // General
        public float ChoiceThreshold { get; set; }
        public float Decision { get; set; }

        // Personal Drift
        public float PersonalDrift { get; set; }
        public float SocialDrift { get; set; }


        // Social Drift
        public int NumberOfResponsesA { get; set; }
        public int NumberOfResponsesB { get; set; }
        public float SocialDriftInfluence { get; set; } // s
        public float SocialDriftQ { get; set; } // q

        public float CumulativeEvidence { get; set; }
        
        // Task
        public float Coherence { get; set; }
        

        public void EstimateCumulativeEvidence()
        {
            // if decision is made skip computation of cumulative evidence
            if (Decision != 0) return;

            SocialDrift = EstimateSocialDriftRate();
            PersonalDrift = EstimatePersonalDriftRate(Coherence);

            CumulativeEvidence = CumulativeEvidence +
                                 (PersonalDrift + SocialDrift) * Time.fixedDeltaTime +
                                 Time.fixedDeltaTime * Utils.Utils.SampleGaussian(0, 1);
        }

        // TODO: negative m is possible??
        float EstimateSocialDriftRate()
        {
            var m = NumberOfResponsesA - NumberOfResponsesB;
            return SocialDriftInfluence * m;  // (float) Math.Pow(m, SocialDriftQ);
        }
        

        // TODO: This is not the correct way to estimate personal drift rate.
        float EstimatePersonalDriftRate(float coherence)
        {
            return 0.53f * coherence + Utils.Utils.SampleGaussian( 0, 1);
        }
    }
}