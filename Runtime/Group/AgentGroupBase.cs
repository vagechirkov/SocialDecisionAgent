using System.Collections.Generic;
using System.Linq;
using SocialDecisionAgent.Runtime.SocialAgent;
using SocialDecisionAgent.Runtime.Utils;
using UnityEngine;

namespace SocialDecisionAgent.Runtime.Group
{
    public class AgentGroupBase: MonoBehaviour, IAgentGroup

    {
        [field: Tooltip("Max Environment Steps")]
        public int MaxEnvironmentSteps { get; set; } = 1000;

        [SerializeField] float fovDist = 20.0f;
        
        [SerializeField] float fovAngle = 45.0f;
        
        const float MovingDotsObservation = 0.75f;

        public GameObject[] AgentGameObjects { get; private set; }
        public ISocialAgent[] Agents { get; private set; }

        [HideInInspector] public int resetTimer;

        void Awake()
        {
            InitializeAgentGroup();
        }

        public void InitializeAgentGroup()
        {
            AgentGameObjects = GameObject.FindGameObjectsWithTag("agent");
            Agents = AgentGameObjects.Select(a => a.GetComponent<ISocialAgent>()).ToArray();
            foreach (var agent in Agents)
            {
                agent.Group = this;
            }

            ResetScene();
            
            // TODO: Remove this
            GetComponent<plotAgentDecisions>().allAgents = Agents;
        }

        void FixedUpdate()
        {
            resetTimer += 1;
            
            if (resetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
            {
                ResetScene();
            }
        }
        
        public void ResetScene()
        {
            resetTimer = 0;
            foreach (var agent in Agents)
            {
                agent.ResetDecisionModel(MovingDotsObservation);
            }
        }
        
        public float[] CollectResponsesInTheFieldOfView(GameObject agent)
        {
            var neighborDecisions = new List<float>();
            for (var i = 0; i < AgentGameObjects.Length; i++)
            {
                var a = AgentGameObjects[i];
                var direction = agent.transform.position - a.transform.position;
                var angle = Vector3.Angle(direction, agent.transform.forward);

                // TODO: additionally use Raycast to check if the agent is in the field of view
                if (direction.magnitude < fovDist && angle < fovAngle && a != gameObject)
                {
                    neighborDecisions.Add(Agents[i].Decision);
                    Debug.DrawRay(a.transform.position, direction, Color.red);
                }
                else
                {
                    neighborDecisions.Add(0f);
                }
            }

            return neighborDecisions.ToArray();
        }
        

    }
}