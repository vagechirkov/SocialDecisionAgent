using System.Collections;
using UnityEngine;
using SocialDecisionAgent.Runtime.Task;
using SocialDecisionAgent.Runtime.Task.DynamicColorMatching;

public class TestDynamicColorMatching : MonoBehaviour
{
    DynamicColorMatchingTask Task { get; set; }

    void Start()
    {
        Task = GetComponentInChildren<DynamicColorMatchingTask>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Task.GenerateSample();
            StartCoroutine(Info());
        }
        
    }

    IEnumerator Info()
    {
        var startTime = Time.time;
        while (Task.IsRunning)
        {
            yield return new WaitForSeconds(0.1f);
            
            // Percentage shown
            Debug.Log($"Percentage of the task shown: {Task.PercentageShown}%; " +
                      $"Time passed {Time.time - startTime}s");
        }

        Debug.Log($"The task is finished in: {Task.FinishedInSeconds}s");
    }
}