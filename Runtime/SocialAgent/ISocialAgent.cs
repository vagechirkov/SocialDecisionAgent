using System.Collections.Generic;
using SocialDecisionAgent.Runtime.Group;
using SocialDecisionAgent.Runtime.SocialAgent.Action;
using SocialDecisionAgent.Runtime.Task;

namespace SocialDecisionAgent.Runtime.SocialAgent
{
    public interface ISocialAgent
    {
        IAgentGroup Group { get; set; }
        
        IAgentAction Action { get; set; }
        
        ITask Task { get; set; }
        
        float Decision { get; set;}
        
        float DecisionThreshold { get; set; }
        
        List<float> ActionHistory { get; set; }

        void ResetDecisionModel(float coherence);

    }
}