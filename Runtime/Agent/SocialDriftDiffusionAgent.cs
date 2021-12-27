using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
        [Tooltip("Threshold of the drift diffusion model")]
        [SerializeField] public float threshold = 3.3f;
        
        public float Decision { get; set; }
        
        public SocialDriftDiffusionGroupController Group { get; set; }
        
        
        readonly Random _random = new Random(Environment.TickCount);
        
        SocialDriftDiffusionModel _sddm;
        
        [HideInInspector] public List<float> actionsHistory = new List<float>();
        
        void Awake()
        {
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
        }


        void FixedUpdate()
        {
            actionsHistory.Add(Decision);
        }
    }
    
}
