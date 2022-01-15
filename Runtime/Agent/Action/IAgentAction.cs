namespace SDM.Agents
{
    public interface IAgentAction
    {
        void ResetAction();

        void PerformAction(float agentDecision);
    }
}