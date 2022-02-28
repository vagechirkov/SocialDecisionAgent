using System.Linq;
using SocialDecisionAgent.Runtime.SocialAgent;
using SocialDecisionAgent.Runtime.Task;
using UnityEngine;

namespace SocialDecisionAgent.Runtime.Group
{
    public class MockGroup : MonoBehaviour, IAgentGroup
    {
        [field: Tooltip("Task object")] [SerializeField]
        GameObject task;

        public ITask Task { get; private set; }

        public int MaxEnvironmentSteps { get; set; }

        public GameObject[] AgentGameObjects { get; private set; }

        public ISocialAgent[] Agents { get; private set; }

        public bool IsTrialRunning { get; set; }

        public void InitializeAgentGroup()
        {
            Task = task.GetComponent<ITask>();

            AgentGameObjects = GameObject.FindGameObjectsWithTag("agent");
            Agents = AgentGameObjects.Select(a => a.GetComponent<ISocialAgent>()).ToArray();
        }

        public virtual void GenerateTrial()
        {
            Task.GenerateSample();
            foreach (var agent in Agents) agent.ResetDecisionModel(Task.Coherence);
        }

        public float[] CollectResponsesInTheFieldOfView(GameObject agent)
        {
            return new float[] { };
        }
    }
}