
namespace SocialDecisionAgent.Runtime.Group
{
    public class SDDMAgentGroupController : AgentGroupBase
    {
        void Start()
        {
            InitializeAgentGroup();
            GenerateTrial();
        }
        
        void FixedUpdate()
        {
            resetTimer += 1;
            if (resetTimer >= MaxEnvironmentSteps)
                GenerateTrial();
        }
    }
}