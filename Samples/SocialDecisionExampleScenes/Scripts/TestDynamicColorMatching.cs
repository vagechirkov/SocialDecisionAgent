using UnityEngine;
using SocialDecisionAgent.Runtime.Task;

public class TestDynamicColorMatching : MonoBehaviour
{
    ITask Task { get; set; }

    void Start()
    {
        Task = GetComponentInChildren<ITask>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Task.GenerateSample();
        }
    }
}