using SocialDecisionAgent.Runtime.Utils;
using Unity.MLAgents;
using UnityEngine;

namespace SocialDecisionAgent.Runtime.Group
{
    public class MLAgentGroupController : AgentGroupBase
    {
        
        SimpleMultiAgentGroup _mlAgentGroup;
        
        void Start()
        {
            InitializeAgentGroup();

            _mlAgentGroup = new SimpleMultiAgentGroup();

            foreach (var agent in Agents)
                if (agent is Agent mlAgent)
                    _mlAgentGroup.RegisterAgent(mlAgent);

            var plotter = GetComponent<plotAgentDecisions>();
            if(plotter != null) 
                plotter.allAgents = Agents;
        }
        
        void FixedUpdate()
        {
            if (!IsTrialRunning) // Input.GetKeyDown(KeyCode.Space)
            {
                IsTrialRunning = true;
                GenerateTrial();
            }

            if (IsTrialRunning)
            {
                resetTimer += 1;
                foreach (var agent in Agents)
                    if (agent is Agent mlAgent)
                        mlAgent.RequestDecision();
                
                if (resetTimer >= MaxEnvironmentSteps)
                {
                    _mlAgentGroup.GroupEpisodeInterrupted();
                    IsTrialRunning = false;
                }
            }
        }


    }
}