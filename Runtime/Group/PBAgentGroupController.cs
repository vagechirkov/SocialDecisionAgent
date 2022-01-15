using SocialDecisionAgent.Runtime.SocialAgent;
using UnityEngine;

namespace SocialDecisionAgent.Runtime.Group
{
    public class PBAgentGroupController : AgentGroupBase
    {
        int TrialCount { get; set; }
        
        void Start()
        {
            InitializeAgentGroup();
            // Read CSV file
            TrialCount = 30;
        }
        
        void FixedUpdate()
        {
            resetTimer += 1;

            if (resetTimer >= MaxEnvironmentSteps)
            {
                TrialCount -= 1;
                GenerateTrial();
            }
        }
        
        public override void GenerateTrial()
        {
            resetTimer = 0;
            Task.GenerateSample();
            TrialCount -= 1;
            for (var i = 0; i < Agents.Length; i++)
            {
                var agent = Agents[i];
                agent.ResetDecisionModel(0);
                if (agent is PrecomputedBehaviorAgent pbAgent)
                {
                    // TODO: read decisions from file
                    pbAgent.Decision = 1;
                    pbAgent.ReactionTime = 4f;
                    pbAgent.StartCoroutine(pbAgent.WaitAndResponse());
                }
            }
        }
        
        
    }
}