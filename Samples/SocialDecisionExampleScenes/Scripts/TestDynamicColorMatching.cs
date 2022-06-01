using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocialDecisionAgent.Runtime.Task;
using SocialDecisionAgent.Runtime.Task.DynamicColorMatching;

public class TestDynamicColorMatching : MonoBehaviour
{
    DynamicColorMatchingTask Task { get; set; }

    List<float> finishTimes = new List<float>();

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
            Debug.Log($"Percentage of the task shown: {(int)(Task.PercentageShown * 100)}%; " +
                      $"Time passed {Time.time - startTime}s");
        }

        Debug.Log($"The task is finished in: {Task.FinishedInSeconds}s");
        finishTimes.Add(Task.FinishedInSeconds);
    }
    
    void OnGUI()
    {
        var guiStyle = new GUIStyle();
        var toggleStyle = new GUIStyle(GUI.skin.toggle);
            
        guiStyle.fontSize = 30;
        toggleStyle.fontSize = 30;
        var color = Color.black;
        color.a = 0.5f;
        guiStyle.normal.textColor = color;


        GUI.Label(
            new Rect(50, 25, 300, 20), "Debug info:", guiStyle);

        var responseColor = Task.Coherence < 0 ? "orange" : "blue";
        if (Task.Coherence == 0) responseColor = "random";
            
        GUI.Label(
            new Rect(50, 75, 300, 20),
            "Coherence: " + (int) (Task.Coherence*100) + " % (" + responseColor + ")",
            guiStyle);
        GUI.Label(
            new Rect(50, 125, 300, 20),
            $"Percentage of the task shown: {(int)(Task.PercentageShown * 100)}%; ",
            guiStyle);

  
        var n = 0;
        foreach (var t in finishTimes)
        {
            GUI.Label(
                new Rect(50, 175 + n * 25, 300, 20),
                $"The task is finished in: {t}s",
                guiStyle);
            n++;
        }


        
    }
}