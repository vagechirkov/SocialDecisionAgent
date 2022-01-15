using System;
using System.Collections.Generic;
using SocialDecisionAgent.Runtime.Group;
using SocialDecisionAgent.Runtime.SocialAgent.Action;
using SocialDecisionAgent.Runtime.Task;
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
        public float Decision { get; set; }

        public float DecisionThreshold { get; set; }

        public IAgentGroup Group { get; set; }

        public IAgentAction Action { get; set; }

        public ITask Task { get; set; }


        [SerializeField] float noiseLevelSensors = 0.01f;
        [SerializeField] bool hideCoherenceInput;
        
        [HideInInspector] public List<float> actionsHistory = new List<float>();

        void Awake()
        {
            ResetDecisionModel(0);
        }

        public void ResetDecisionModel(float coherence)
        {
            Decision = 0;
        }


        public override void CollectObservations(VectorSensor sensor)
        {
            if (hideCoherenceInput)
                sensor.AddObservation(0);
            else
                sensor.AddObservation(Utils.Utils.SampleGaussian(Task.Coherence, noiseLevelSensors));
            var neighbors = Group.CollectResponsesInTheFieldOfView(gameObject);
            foreach (var n in neighbors) sensor.AddObservation(n);
        }

        public override void OnActionReceived(ActionBuffers actionBuffers)
        {
            if (Decision == 0)
            {
                var newDecision = actionBuffers.ContinuousActions[0];
                if (Math.Abs(newDecision) >= DecisionThreshold)
                {
                    Decision = Math.Sign(newDecision);
                    Action.PerformAction(Decision);
                }

                AddReward(-1f / Group.MaxEnvironmentSteps);

                if (Task.Coherence < 0 && Decision < 0 ||
                    Task.Coherence > 0 && Decision > 0)
                    AddReward(1.0f);
                else if (Task.Coherence < 0 && Decision > 0 ||
                         Task.Coherence > 0 && Decision < 0)
                    AddReward(-1.0f);

                actionsHistory.Add(newDecision);
            }
            else
            {
                actionsHistory.Add(Decision);
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