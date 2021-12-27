using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SDM.Agents
{
    public class SocialDriftDiffusionAgentAction : MonoBehaviour
    {
        Material agentMaterial;

        void Awake()
        {
            agentMaterial = GetComponent<MeshRenderer>().material;
        }


        public void UpdateAgentColor(int agentDecision)
        {
            agentMaterial.color = agentDecision switch
            {
                -1 => Color.red,
                1 => Color.green,
                _ => agentMaterial.color
            };
        }
    }
}