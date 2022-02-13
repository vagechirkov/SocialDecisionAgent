using UnityEngine;
using SocialDecisionAgent.Runtime.Group;

public class ExperimentManager : MonoBehaviour
{
    [SerializeField] SDDMAgentGroupController group;

    void Update()
    {
        if(!group.IsTrialRunning & Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(group.RunTrial());
        }
    }
}
