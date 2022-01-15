using SocialDecisionAgent.Runtime.Utils;
using UnityEngine;

namespace SocialDecisionAgent.Runtime.Group
{
    public class SDDMAgentGroupController : AgentGroupBase
    {
        void Awake()
        {
            InitializeAgentGroup();
            var plotter = GetComponent<plotAgentDecisions>();
            if(plotter != null) 
                plotter.allAgents = Agents;
        }
        
        void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !IsTrialRunning)
            {
                IsTrialRunning = true;
                GenerateTrial();
            }
            
            if (IsTrialRunning)
            {
                resetTimer += 1;
                if (resetTimer >= MaxEnvironmentSteps)
                {
                    GenerateTrial();
                    IsTrialRunning = false;
                }
            }
        }
        
        
    }
}