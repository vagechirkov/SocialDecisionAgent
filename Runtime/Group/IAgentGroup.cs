using UnityEngine;

namespace SocialDecisionAgent.Runtime.Group
{
    public interface IAgentGroup
    {
        int MaxEnvironmentSteps { get; set; }
        
        float[] CollectResponsesInTheFieldOfView(GameObject agent);
    }
}