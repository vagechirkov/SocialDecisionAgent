using SDM.Group;

namespace SDM.Agents
{
    public interface ISocialAgent
    {
        float Decision { get; set;}
        
        IAgentGroup Group { get; set; }
        
        IAgentAction Action { get; set; }
        
        void ResetDecisionModel(float coherence);

    }
}