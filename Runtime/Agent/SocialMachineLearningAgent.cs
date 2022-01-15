using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SDM.Group;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using Random = System.Random;

namespace SDM.Agents
{
    
    /// <summary>
    /// https://github.com/Unity-Technologies/ml-agents/blob/main/docs/Learning-Environment-Create-New.md
    /// https://github.com/Unity-Technologies/ml-agents/blob/main/docs/Learning-Environment-Design-Agents.md
    /// </summary>
    public class SocialMachineLearningAgent : Agent
    {

        [SerializeField] float noiseLevelSensors = 0.01f;
        [SerializeField] float neighborsInfluenceDDM = 0.01f;
        [SerializeField] public float thresholdDDM = 1;
        [SerializeField] bool hideCoherenceInput;
        
        [SerializeField] bool playerControl;
        
        public float agentDecision;
        public float movingDotsCoherence;
        [HideInInspector] public Material agentMaterial;
        [HideInInspector] public List<float> actionsHistory = new List<float>();
        public MachineLearningGroupController Group { get; set; }
        
        TMP_Text _infoText;
        
        readonly Random _random = new Random(Environment.TickCount);
        
        SocialDriftDiffusionModel _ddm;
        
        
        void Awake()
        {
            agentMaterial = GetComponent<MeshRenderer>().material;
            _infoText = GetComponentInChildren<TMP_Text>();

            ResetDDM();
        }
        
        public void ResetDDM()
        {
            _ddm = new SocialDriftDiffusionModel
            {
                ChoiceThreshold = thresholdDDM,
                NumberOfResponsesA = 0,
                NumberOfResponsesB = 0,
                SocialDriftInfluence = neighborsInfluenceDDM,
                SocialDriftQ = 0.66f,
                CumulativeEvidence = 0
            };
            agentDecision = 0;
        }
        
        public override void CollectObservations(VectorSensor sensor)
        {
            if (hideCoherenceInput)
                sensor.AddObservation(0);
            else
                sensor.AddObservation(Utils.Utils.SampleGaussian(movingDotsCoherence, noiseLevelSensors));
            var neighbors = Group.CollectResponsesInTheFieldOfView(gameObject);
            foreach (var n in neighbors) sensor.AddObservation(n);
        }
        
        public override void OnActionReceived(ActionBuffers actionBuffers)
        {
            if (agentDecision == 0)
            {
                var newDecision = actionBuffers.ContinuousActions[0];
                if (newDecision <= -thresholdDDM)
                {
                    agentMaterial.color = Color.red;
                    agentDecision = -1;
                }
                else if (newDecision >= thresholdDDM)
                {
                    agentMaterial.color = Color.green;
                    agentDecision = 1;
                }

                AddReward(-1f / Group.maxEnvironmentSteps);

                if (movingDotsCoherence < 0 && agentDecision < 0 ||
                    movingDotsCoherence > 0 && agentDecision > 0)
                {
                    AddReward(1.0f);
                }
                else if (movingDotsCoherence < 0 && agentDecision > 0 ||
                         movingDotsCoherence > 0 && agentDecision < 0)
                {
                    AddReward(-1.0f);
                }

                _infoText.text = Math.Round(newDecision, 2).ToString(CultureInfo.InvariantCulture);
                actionsHistory.Add(newDecision);
            }
            else
            {
                actionsHistory.Add(agentDecision);
            }
        }
        
        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var continuousActions = actionsOut.ContinuousActions;

            if (playerControl)
                continuousActions[0] = PlayerDecision();
            else
            {
                _ddm.PersonalDrift = movingDotsCoherence;
                var neighbors = Group.CollectResponsesInTheFieldOfView(gameObject);
                _ddm.NumberOfResponsesA = neighbors.Count(n => Math.Abs(n + 1) < 0.01);
                _ddm.NumberOfResponsesB = neighbors.Count(n => Math.Abs(n - 1) < 0.01);
                _ddm.CumulativeEvidence = _ddm.CumulativeEvidence;
                continuousActions[0] = _ddm.CumulativeEvidence;
            }
        }

        float PlayerDecision()
        {
            var newDecision = Input.GetAxis("Horizontal");
            return newDecision;
        }
    }


}