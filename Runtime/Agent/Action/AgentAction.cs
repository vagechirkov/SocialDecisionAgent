using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SDM.Agents
{
    public class AgentAction : MonoBehaviour, IAgentAction
    {
        Material agentMaterial;

        void Awake()
        {
            agentMaterial = GetComponent<MeshRenderer>().material;
        }

        public void PerformAction(float agentDecision)
        {
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