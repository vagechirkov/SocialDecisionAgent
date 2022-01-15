namespace SocialDecisionAgent.Runtime.SocialAgent.Action
{
    public interface IAgentAction
    {
        void ResetAction();

        void PerformAction(float agentDecision);
    }
}