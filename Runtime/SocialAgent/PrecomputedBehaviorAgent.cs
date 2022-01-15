using System.Collections;
using System.Collections.Generic;
using SocialDecisionAgent.Runtime.Group;
using SocialDecisionAgent.Runtime.SocialAgent.Action;
using UnityEngine;

namespace SocialDecisionAgent.Runtime.SocialAgent
{
    public class PrecomputedBehaviorAgent : MonoBehaviour, ISocialAgent
    {
        public IAgentGroup Group { get; set; }
        
        public IAgentAction Action { get; set; }
        
        public float Decision { get; set; }
        
        public float DecisionThreshold { get; set; }
        
        public List<float> ActionHistory { get; set; }
        
        void Awake()
        {
            Action = GetComponentInChildren<IAgentAction>();
        }
        
        public void ResetDecisionModel(float coherence)
        {
            Decision = 0;
            Action.ResetAction();
        }
        
        public IEnumerator WaitAndResponse(float reactionTime)
        {
            yield return new WaitForSeconds(reactionTime);
            Action.PerformAction(Decision);
        }
    }
}