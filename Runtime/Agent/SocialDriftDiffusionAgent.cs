using System;
using System.Collections.Generic;
using System.Linq;
using SDM.Group;
using UnityEngine;

namespace SDM.Agents
{
    public class SocialDriftDiffusionAgent : MonoBehaviour, ISocialAgent
    {
        
        // Model parameters
        [Tooltip("Scaling parameter of the social drift rate")]
        [SerializeField] float socialDriftInfluence = 0.36f;
        
        [Tooltip("Power of the majority size")]
        [SerializeField] float socialDriftQ = 0.66f;

        [Tooltip("Threshold of the drift diffusion model")] [SerializeField]
        public float threshold = 1;// 3.3f;
        
        public float Decision { get; set; }
        
        public IAgentGroup Group { get; set; }

        public IAgentAction Action { get; set; }

        public SocialDriftDiffusionModel sddm;
        
        [HideInInspector] public List<float> actionsHistory = new List<float>();
        
        void Awake()
        {
            Action = GetComponent<IAgentAction>();
            ResetDecisionModel(0);
        }
        
        public void ResetDecisionModel(float coherence)
        {
            sddm = new SocialDriftDiffusionModel
            {
                ChoiceThreshold = threshold,
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
            
            actionsHistory.Add(sddm.CumulativeEvidence);
            if (Decision == 0)
            {
                Decision = Math.Abs(sddm.CumulativeEvidence) >= threshold ? Math.Sign(sddm.CumulativeEvidence) : 0;
                sddm.Decision = Decision;

                Action.PerformAction(Decision);
            }
        }
    }
    
}
