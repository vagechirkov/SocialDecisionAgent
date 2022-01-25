using System;
using System.Collections.Generic;
using SocialDecisionAgent.Runtime.Group;
using SocialDecisionAgent.Runtime.SocialAgent.Action;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace SocialDecisionAgent.Runtime.SocialAgent
{
    /// <summary>
    ///     https://github.com/Unity-Technologies/ml-agents/blob/main/docs/Learning-Environment-Create-New.md
    ///     https://github.com/Unity-Technologies/ml-agents/blob/main/docs/Learning-Environment-Design-Agents.md
    /// </summary>
    public class SocialMachineLearningAgent : Agent, ISocialAgent
    {
        
        [SerializeField] float noiseLevelSensors = 0.01f;
        
        [SerializeField] bool hideCoherenceInput;
        
        public float Decision { get; set; }

        public float DecisionThreshold { get; set; } = 1f;

        public IAgentGroup Group { get; set; }

        public IAgentAction Action { get; set; }
        
        public List<float> ActionHistory { get; set; } = new List<float>();

        public void Awake()
        {
            Action = GetComponentInChildren<IAgentAction>();
        }

        public void ResetDecisionModel(float coherence)
        {
            Decision = 0;
            Action.ResetAction();
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            if (hideCoherenceInput)
                sensor.AddObservation(transform.position.x);
            else
                sensor.AddObservation(Utils.Utils.SampleGaussian(Group.Task.Coherence, noiseLevelSensors));
            var neighbors = Group.CollectResponsesInTheFieldOfView(gameObject);
            foreach (var n in neighbors) sensor.AddObservation(n);
        }

        public override void OnActionReceived(ActionBuffers actionBuffers)
        {
            // FIXME: this is not the right way to do it
            var coherence = Group.Task.Coherence;
            if (Decision == 0)
            {
                var newDecision = actionBuffers.ContinuousActions[0];
                if (Math.Abs(newDecision) >= DecisionThreshold)
                {
                    Decision = Math.Sign(newDecision);
                    Action.PerformAction(Decision);
                }

                AddReward(-1f / Group.MaxEnvironmentSteps);

                if (coherence < 0.5 && Decision < 0 || coherence > 0.5 && Decision > 0)
                    AddReward(1.0f);
                else if (coherence < 0.5 && Decision > 0 || coherence > 0.5 && Decision < 0)
                    AddReward(-1.0f);

                ActionHistory.Add(newDecision);
            }
            else
            {
                ActionHistory.Add(Decision);
            }
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var continuousActions = actionsOut.ContinuousActions;

            continuousActions[0] = PlayerDecision();
        }

        float PlayerDecision()
        {
            var newDecision = Input.GetAxis("Horizontal");
            return newDecision;
        }
    }
}