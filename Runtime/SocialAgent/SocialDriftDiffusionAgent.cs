using System.Collections.Generic;
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

        // Decision-making model
        IAgentModel Model { get; set; }
        
        bool actionTaken = false;
        
        void Awake()
        {
            Action = GetComponent<IAgentAction>();
            Model = GetComponent<IAgentModel>();
        }

        public void ResetDecisionModel(float coherence)
        {
            Decision = 0;
            actionTaken = false;
            Model.ResetModel(coherence);
            Action.ResetAction();
        }

        void FixedUpdate()
        {
            if (Group.IsTrialRunning)
            {
                Model.UpdateModel(Group.CollectResponsesInTheFieldOfView(gameObject));
                ActionHistory.Add(Model.CumulativeEvidence);
                if (actionTaken) return;
                
                if (Model.Decision != 0)
                {
                    Decision = Model.Decision;
                    actionTaken = true;
                    Action.PerformAction(Model.Decision);
                }
                
            }
        }
    }
}