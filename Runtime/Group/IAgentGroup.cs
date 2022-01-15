using SocialDecisionAgent.Runtime.SocialAgent;
using SocialDecisionAgent.Runtime.Task;
using UnityEngine;

namespace SocialDecisionAgent.Runtime.Group
{
    public interface IAgentGroup
    {
        int MaxEnvironmentSteps { get; set; }
        
        ISocialAgent [] Agents { get; }
        
        ITask Task { get; }
        
        bool IsTrialRunning { get; set; }
        
        GameObject[] AgentGameObjects { get; }

        void InitializeAgentGroup();
        
        float[] CollectResponsesInTheFieldOfView(GameObject agent);

        void GenerateTrial();


    }
}