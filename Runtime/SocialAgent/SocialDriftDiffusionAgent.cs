using System;
using System.Collections.Generic;
using System.Linq;
using SocialDecisionAgent.Runtime.Group;
using SocialDecisionAgent.Runtime.SocialAgent.Action;
using SocialDecisionAgent.Runtime.SocialAgent.Model;
using UnityEngine;

namespace SocialDecisionAgent.Runtime.SocialAgent
{
    public class SocialDriftDiffusionAgent : MonoBehaviour, ISocialAgent
    {
        public float Decision { get; set; }
        
        public float DecisionThreshold { get; set; }

        public IAgentGroup Group { get; set; }

        public IAgentAction Action { get; set; }
        
        public List<float> ActionHistory { get; set; } = new List<float>();

        // Social Drift Diffusion model
        SocialDriftDiffusionModel sddm { get; set; }
        
        // social drift diffusion model parameters
        [Tooltip("Scaling parameter of the social drift rate")] [SerializeField]
        float socialDriftInfluence = 0.36f;

        [Tooltip("Power of the majority size")] [SerializeField]
        float socialDriftQ = 0.66f;
        
        void Awake()
        {
            Action = GetComponentInChildren<IAgentAction>();
            ResetDecisionModel(0);
        }

        public void ResetDecisionModel(float coherence)
        {
            sddm = new SocialDriftDiffusionModel
            {
                ChoiceThreshold = DecisionThreshold,
                NumberOfResponsesA = 0,
                NumberOfResponsesB = 0,
                SocialDriftInfluence = socialDriftInfluence,
                SocialDriftQ = socialDriftQ,
                CumulativeEvidence = 0
            };
            Decision = 0;
            sddm.Coherence = coherence;
            Action.ResetAction();
        }

        void FixedUpdate()
        {
            var neighbors = Group.CollectResponsesInTheFieldOfView(gameObject);
            sddm.NumberOfResponsesA = neighbors.Count(n => Math.Abs(n - 1) < 0.01);
            sddm.NumberOfResponsesB = neighbors.Count(n => Math.Abs(n + 1) < 0.01);
            sddm.EstimateCumulativeEvidence();

            ActionHistory.Add(sddm.CumulativeEvidence);
            
            if (Decision != 0) return;
            
            Decision = Math.Abs(sddm.CumulativeEvidence) >= DecisionThreshold ? Math.Sign(sddm.CumulativeEvidence) : 0;
            sddm.Decision = Decision;

            Action.PerformAction(Decision);
        }
    }
}