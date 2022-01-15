
namespace SocialDecisionAgent.Runtime.Group
{
    public class SDDMAgentGroupController : AgentGroupBase
    {
        void Awake()
        {
            InitializeAgentGroup();
        }
        
        void FixedUpdate()
        {
            resetTimer += 1;
            if (resetTimer >= MaxEnvironmentSteps)
                GenerateTrial();
        }
    }
}