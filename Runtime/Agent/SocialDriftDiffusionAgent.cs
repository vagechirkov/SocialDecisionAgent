using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SDM.Group;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace SDM.Agents
{
    public class SocialDriftDiffusionAgent : MonoBehaviour
    {
        
        // Model parameters
        [Tooltip("Scaling parameter of the social drift rate")]
        [SerializeField] float socialDriftInfluence = 0.36f;
        
        [Tooltip("Power of the majority size")]
        [SerializeField] float socialDriftQ = 0.66f;

        [Tooltip("Threshold of the drift diffusion model")] [SerializeField]
        public float threshold = 1;// 3.3f;
        
        public float Decision { get; set; }
        
        public SocialDriftDiffusionGroupController Group { get; set; }

        readonly Random _random = new Random((int)DateTime.Now.Ticks);

        SocialDriftDiffusionModel _sddm;
        AgentAction _action;
        
        [HideInInspector] public List<float> actionsHistory = new List<float>();
        
        void Awake()
        {
            _action = GetComponent<AgentAction>();
            ResetDecisionModel();
        }
        
        public void ResetDecisionModel()
        {
            _sddm = new SocialDriftDiffusionModel
            {
                ChoiceThreshold = threshold,
                NumberOfResponsesA = 0,
                NumberOfResponsesB = 0,
                SocialDriftInfluence = socialDriftInfluence,
                SocialDriftQ = socialDriftQ,
                Rand = _random,
                CumulativeEvidence = 0
            };
            Decision = 0;
            _action.ResetAction();
        }


        void FixedUpdate()
        {
            var neighbors = Group.CollectResponsesInTheFieldOfView(gameObject);
            _sddm.NumberOfResponsesA = neighbors.Count(n => Math.Abs(n + 1) < 0.01);
            _sddm.NumberOfResponsesB = neighbors.Count(n => Math.Abs(n - 1) < 0.01);
            _sddm.CumulativeEvidence = _sddm.CumulativeEvidence;

            Decision = Math.Abs(_sddm.CumulativeEvidence) > threshold ? Math.Sign(_sddm.CumulativeEvidence) : 0;

            _action.UpdateAgentColor(Decision);
            
            actionsHistory.Add(_sddm.CumulativeEvidence);
        }
    }
    
}
