namespace SocialDecisionAgent.Runtime.SocialAgent.Model
{
    public interface IAgentModel
    {
        float Coherence { get; set; }
        
        float CumulativeEvidence { get; set; }
        
        float Decision { get; set; }
        
        void ResetModel(float coherence);
        
        void UpdateModel(float[] neighborResponses);
        
        
        
        
    }
}