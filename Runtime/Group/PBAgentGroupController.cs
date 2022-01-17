using System.Globalization;
using SocialDecisionAgent.Runtime.SocialAgent;
using UnityEngine;

namespace SocialDecisionAgent.Runtime.Group
{
    public class PBAgentGroupController : AgentGroupBase
    {
        [SerializeField] string pathToBehaviourData;

        string[][] BehaviourData { get; set; }
        
        int TrialCount { get; set; }
        
        void Awake()
        {
            InitializeAgentGroup();
            BehaviourData = Utils.Utils.ReadCsvFile(pathToBehaviourData);
            TrialCount = 0;
        }
        
        void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !IsTrialRunning)
            {
                IsTrialRunning = true;
                GenerateTrial();
            }
            
            if (IsTrialRunning)
            {
                resetTimer += 1;
                if (resetTimer >= MaxEnvironmentSteps)
                {
                    foreach (var agent in Agents) agent.ResetDecisionModel(0);
                    IsTrialRunning = false;
                }
            }
        }
        
        public override void GenerateTrial()
        {
            resetTimer = 0;
            Task.GenerateSample();
            TrialCount += 1;
            for (var i = 0; i < Agents.Length; i++)
            {
                var agent = Agents[i];
                agent.ResetDecisionModel(0);
                if (agent is PrecomputedBehaviorAgent pbAgent)
                {
                    // TODO: read decisions from file
                    pbAgent.Decision = Random.value - 0.5f;
                    var reactionTime = float.Parse(
                        BehaviourData[TrialCount][i], CultureInfo.InvariantCulture.NumberFormat);
                    pbAgent.StartCoroutine(pbAgent.WaitAndResponse(reactionTime));
                }
            }
        }
        
        
    }
}