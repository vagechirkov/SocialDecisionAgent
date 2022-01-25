using UnityEngine;


namespace SocialDecisionAgent.Runtime.SocialAgent.Action
{
    public class CapsuleAgentAction : MonoBehaviour, IAgentAction
    {
        Material agentMaterial;

        void Awake()
        {
            agentMaterial = GetComponent<MeshRenderer>().material;
        }

        public void PerformAction(float agentDecision)
        {
            // agentDecision = Mathf.Sign(agentDecision);
            
            agentMaterial.color = agentDecision switch
            {
                -1 => Color.red,
                1 => Color.green,
                _ => agentMaterial.color
            };
        }

        public void ResetAction()
        {
            agentMaterial.color = Color.blue;
        }
    }
}