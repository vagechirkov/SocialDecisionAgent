using System.Collections.Generic;
using System.Linq;
using SocialDecisionAgent.Runtime.SocialAgent;
using SocialDecisionAgent.Runtime.Task;
using SocialDecisionAgent.Runtime.Utils;
using UnityEngine;

namespace SocialDecisionAgent.Runtime.Group
{
    public class AgentGroupBase : MonoBehaviour, IAgentGroup

    {
        [field: Tooltip("Max Environment Steps")] [SerializeField]
        public int maxEnvironmentSteps = 500;

        [field: Tooltip("Task object")] [SerializeField] 
        GameObject task;
        
        [SerializeField] float fovDist = 20.0f;

        [SerializeField] float fovAngle = 45.0f;
        
        public int resetTimer;
        
        public ITask Task { get; private set; }

        public int MaxEnvironmentSteps { get; set; }
        
        public GameObject[] AgentGameObjects { get; private set; }
        public ISocialAgent[] Agents { get; private set; }

        public void InitializeAgentGroup()
        {
            Task = task.GetComponent<ITask>();
            MaxEnvironmentSteps = maxEnvironmentSteps;
            
            AgentGameObjects = GameObject.FindGameObjectsWithTag("agent");
            Agents = AgentGameObjects.Select(a => a.GetComponent<ISocialAgent>()).ToArray();
            foreach (var agent in Agents) agent.Group = this;
            
            // TODO: Remove this
            GetComponent<plotAgentDecisions>().allAgents = Agents;
        }
        
        public virtual void GenerateTrial()
        {
            resetTimer = 0;
            Task.GenerateSample();
            foreach (var agent in Agents) agent.ResetDecisionModel(Task.Coherence);
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