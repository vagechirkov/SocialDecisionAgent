using UnityEngine;

namespace SDM.Group
{
    public interface IAgentGroup
    {
        int MaxEnvironmentSteps { get; set; }
        
        float[] CollectResponsesInTheFieldOfView(GameObject agent);
    }
}