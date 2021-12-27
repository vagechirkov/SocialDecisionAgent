using System;
using UnityEngine;
using Random = System.Random;

namespace SDM.Agents
{
    public class SocialDriftDiffusionModel
    {
        // General
        public float ChoiceThreshold { get; set; }
        public float Decision;
        
        // Personal Drift
        public float PersonalDrift
        {
            get => _personalDriftRate; 
            set => EstimatePersonalDriftRate(value);
        }
        
        
        // Social Drift
        public int NumberOfResponsesA { get; set; }
        public int NumberOfResponsesB { get; set; }
        public float SocialDriftInfluence { get; set; } // s
        public float SocialDriftQ { get; set; } // q
        public Random Rand { get; set; }

        public float CumulativeEvidence
        {
            get => _currentCumulativeEvidence;
            set => _currentCumulativeEvidence = EstimateCumulativeEvidence(value);
        }
        
        float _currentCumulativeEvidence, _personalDriftRate, _socialDriftRate;


        float EstimateCumulativeEvidence(float l)
        {
            _socialDriftRate = EstimateSocialDriftRate();
            
            l = l + (PersonalDrift + _socialDriftRate) * Time.fixedDeltaTime +
                Time.fixedDeltaTime * (float) Utils.SampleGaussian(Rand, 0, 1);
            return l;
        }

        float EstimateSocialDriftRate()
        {
            var m = NumberOfResponsesA - NumberOfResponsesB;
            return SocialDriftInfluence * (float) Math.Pow(m, SocialDriftQ);
        }
        

        // TODO: This is not the correct way to estimate personal drift rate.
        float EstimatePersonalDriftRate(float coherence)
        {
            return 0.53f * coherence + (float) Utils.SampleGaussian(Rand, 0, 1);
        }
    }
}