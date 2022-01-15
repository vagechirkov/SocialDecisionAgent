using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SDM.Agents;
using SDM.Utils;
using UnityEngine;

namespace SDM.Group
{
    public class SocialDriftDiffusionGroupController : MonoBehaviour, IAgentGroup
    {
        [field: Tooltip("Max Environment Steps")]
        public int MaxEnvironmentSteps { get; set; } = 1000;

        [SerializeField] float fovDist = 20.0f;
        [SerializeField] float fovAngle = 45.0f;
        
        const float MovingDotsObservation = 0.75f;
        
        GameObject[] _agentGroup;
        SocialDriftDiffusionAgent[] _agentGroupScrips;

        [HideInInspector] public int resetTimer;

        void Awake()
        {
            _agentGroup = GameObject.FindGameObjectsWithTag("agent");
            _agentGroupScrips = _agentGroup.Select(a => a.GetComponent<SocialDriftDiffusionAgent>()).ToArray();
            foreach (var agent in _agentGroupScrips)
            { 
                agent.Group = this;
            }

            ResetScene();
            GetComponent<plotAgentDecisions>().allAgents = _agentGroupScrips;
        }

        public float[] CollectResponsesInTheFieldOfView(GameObject agent)
        {
            var neighborDecisions = new List<float>();
            for (var i = 0; i < _agentGroup.Length; i++)
            {
                var a = _agentGroup[i];
                var direction = agent.transform.position - a.transform.position;
                var angle = Vector3.Angle(direction, agent.transform.forward);

                // TODO: additionally use Raycast to check if the agent is in the field of view
                if (direction.magnitude < fovDist && angle < fovAngle && a != gameObject)
                {
                    neighborDecisions.Add(_agentGroupScrips[i].Decision);
                    Debug.DrawRay(a.transform.position, direction, Color.red);
                }
                else
                {
                    neighborDecisions.Add(0f);
                }
            }

            return neighborDecisions.ToArray();
        }

        void FixedUpdate()
        {
            resetTimer += 1;
            // var agentDecisions = _agentGroupScrips.Select(a => a.sddm.CumulativeEvidence).ToArray();
            // Debug.Log(string.Join(" ", agentDecisions));
            if (resetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
            {
                ResetScene();
            }
        }

        void ResetScene()
        {
            resetTimer = 0;
            foreach (var agent in _agentGroupScrips)
            {
                // agent.movingDotsCoherence = movingDotsObservation;
                
                agent.ResetDecisionModel(MovingDotsObservation);
            }
        }
    }
}