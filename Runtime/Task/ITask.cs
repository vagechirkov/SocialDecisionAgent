namespace SocialDecisionAgent.Runtime.Task
{
    public interface ITask
    {
        
        float Coherence { get; set; }

        void GenerateSample();

    }
}