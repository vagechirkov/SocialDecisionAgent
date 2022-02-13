using UnityEngine;
using System.Linq;

namespace SocialDecisionAgent.Runtime.SocialAgent.Model
{
    public class SocialDriftDiffusionModel: MonoBehaviour, IAgentModel
    {
        // General
        [Tooltip("Decision threshold for the social drift diffusion model")] [SerializeField]
        float threshold = 1f;
        
        public float Decision { get; set; }
        
        public float CumulativeEvidence { get; set; }

        // Personal Drift
        float PersonalDrift { get; set; }

        // Social Drift
        float SocialDrift { get; set; }
        
        int NumberOfResponsesA { get; set; }

        int NumberOfResponsesB { get; set; }
        
        [Tooltip("Scaling parameter of the social drift rate")] [SerializeField]
        float socialDriftInfluence = 0.36f;

        [Tooltip("Power of the majority size")] [SerializeField]
        float socialDriftQ = 0.66f;
        
        // Task
        public float Coherence { get; set; }
        
        
        public void ResetModel(float coherence)
        {
            Coherence = coherence;
            Decision = 0;
            NumberOfResponsesA = 0;
            NumberOfResponsesB = 0;
            CumulativeEvidence = 0;
            PersonalDrift = 0;
            SocialDrift = 0;
        }

        public void UpdateModel(float[] neighbors)
        {
            NumberOfResponsesA = neighbors.Count(n => Mathf.Abs(n - 1) < 0.01);
            NumberOfResponsesB = neighbors.Count(n => Mathf.Abs(n + 1) < 0.01);
            EstimateCumulativeEvidence();
        }


        void EstimateCumulativeEvidence()
        {
            // if decision is made skip computation of cumulative evidence
            if (Decision != 0) return;

            SocialDrift = EstimateSocialDriftRate();
            PersonalDrift = EstimatePersonalDriftRate(Coherence);

            CumulativeEvidence = CumulativeEvidence +
                                 (PersonalDrift + SocialDrift) * Time.fixedDeltaTime +
                                 Time.fixedDeltaTime * Utils.Utils.SampleGaussian(0, 1);
            Decision = Mathf.Abs(CumulativeEvidence) >= threshold ? Mathf.Sign(CumulativeEvidence) : 0;
        }

        // TODO: negative m is possible??
        float EstimateSocialDriftRate()
        {
            var m = NumberOfResponsesA - NumberOfResponsesB;
            return socialDriftInfluence * m;  // (float) Math.Pow(m, socialDriftQ);
        }
        

        // TODO: This is not the correct way to estimate personal drift rate.
        float EstimatePersonalDriftRate(float coherence)
        {
            return coherence + Utils.Utils.SampleGaussian( 0, 1);
        }
    }
}