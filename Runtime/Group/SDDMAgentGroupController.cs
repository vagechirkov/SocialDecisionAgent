using System.Collections;
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

        public IEnumerator RunTrial()
        {
            GenerateTrial();
            IsTrialRunning = true;
            while (resetTimer < MaxEnvironmentSteps)
            {
                resetTimer++;
                yield return new WaitForFixedUpdate();
            }
            IsTrialRunning = false;
        }
        
    }
}