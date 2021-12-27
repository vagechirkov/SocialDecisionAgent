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
        
        [SerializeField] float socialDriftInfluence = 0.01f;
        [SerializeField] float socialDriftQ = 0.66f;
        [SerializeField] public float threshold = 1;

        [SerializeField] bool playerControl;
        
        public float agentDecision;
        [HideInInspector] public List<float> actionsHistory = new List<float>();
        public MachineLearningGroupController Group { get; set; }
        
        TMP_Text _infoText;
        
        readonly Random _random = new Random(Environment.TickCount);
        
        SocialDriftDiffusionModel _ddm;
        
        
        void Awake()
        {
            _infoText = GetComponentInChildren<TMP_Text>();
            ResetDecisionModel();
        }
        
        public void ResetDecisionModel()
        {
            _ddm = new SocialDriftDiffusionModel
            {
                ChoiceThreshold = threshold,
                NumberOfResponsesA = 0,
                NumberOfResponsesB = 0,
                SocialDriftInfluence = socialDriftInfluence,
                SocialDriftQ = socialDriftQ,
                Rand = _random,
                CumulativeEvidence = 0
            };
            agentDecision = 0;
        }
        

        float PlayerDecision()
        {
            var newDecision = Input.GetAxis("Horizontal");
            return newDecision;
        }
    }
    
}
