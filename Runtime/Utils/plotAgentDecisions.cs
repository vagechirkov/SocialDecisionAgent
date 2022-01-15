using System.Collections.Generic;
using SocialDecisionAgent.Runtime.Agents;
using UnityEngine;
using UnityEngine.Rendering;


namespace SocialDecisionAgent.Runtime.Utils
{
    
    /// <summary>
    /// Debug plots of agents' decision making process
    /// </summary>
    public class plotAgentDecisions : MonoBehaviour
    {
        public SocialDriftDiffusionAgent[] allAgents;

        Material onguiMat;
        Rect windowRect = new Rect(20, 20, 1500, 1000); // Screen.height - 500
        readonly List<Color> colors = new List<Color>();
        bool showWindow;
        int _numberOfSteps;

        void Start()
        {
            if (!onguiMat)
            {
                // Unity has a built-in shader that is useful for drawing
                // simple colored things. In this case, we just want to use
                // a blend mode that inverts destination colors.
                var shader = Shader.Find("Hidden/Internal-Colored");
                onguiMat = new Material(shader);
                onguiMat.hideFlags = HideFlags.HideAndDontSave;
                // Set blend mode to invert destination colors.
                onguiMat.SetInt("_SrcBlend", (int) BlendMode.OneMinusDstColor);
                onguiMat.SetInt("_DstBlend", (int) BlendMode.Zero);
                // Turn off backface culling, depth writes, depth test.
                onguiMat.SetInt("_Cull", (int) CullMode.Off);
                onguiMat.SetInt("_ZWrite", 0);
                onguiMat.SetInt("_ZTest", (int) CompareFunction.Always);
            }

            foreach (var a in allAgents) colors.Add(Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));

            _numberOfSteps = allAgents[0].Group.MaxEnvironmentSteps;
        }

        void OnGUI()
        {
            showWindow = GUI.Toggle(new Rect(10, 10, 100, 20), showWindow, "Show Graph");
            if (showWindow) windowRect = GUI.Window(0, windowRect, DrawGraph, "");
        }

        /// <summary>
        ///     Draws a basic oscilloscope type graph in a GUI.Window()
        ///     Michael Hutton May 2020
        ///     https://stackoverflow.com/questions/37137110/creating-graphs-in-unity
        /// </summary>
        void DrawGraph(int windowID)
        {
            // Make Window Draggable
            GUI.DragWindow(new Rect(0, 0, 10000, 10000));

            // Draw the graph in the repaint cycle
            if (Event.current.type == EventType.Repaint)
            {
                GL.PushMatrix();

                GL.Clear(true, false, Color.gray);
                onguiMat.SetPass(0);

                // Draw a black back ground Quad 
                float quadX = 4;
                float quadY = 4;
                float quadZ = 0;
                GL.Begin(GL.QUADS);
                GL.Color(Color.gray);
                GL.Vertex3(quadX, quadY, quadZ);
                GL.Vertex3(windowRect.width - quadX, quadY, quadZ);
                GL.Vertex3(windowRect.width - quadX, windowRect.height - quadY, quadZ);
                GL.Vertex3(quadX, windowRect.height - quadY, quadZ);
                GL.End();

                var scale = 200f;
                // Draw threshold lines
                GL.Begin(GL.LINES);
                GL.Color(Color.black);
                GL.Vertex3(0, windowRect.height / 2 + allAgents[0].threshold * scale, 0);
                GL.Vertex3(windowRect.width, windowRect.height / 2 + allAgents[0].threshold * scale, 0);

                GL.Vertex3(0, windowRect.height / 2 - allAgents[0].threshold * scale, 0);
                GL.Vertex3(windowRect.width, windowRect.height / 2 - allAgents[0].threshold * scale, 0);
                GL.End();

                // Draw the lines of the graph
                for (var ai = 0; ai < allAgents.Length; ai++)
                {
                    var agent = allAgents[ai];
                    GL.Begin(GL.LINES);
                    GL.Color(colors[ai]);

                    var valueIndex = agent.actionsHistory.Count - 1;
                    for (var i = (int) (windowRect.width - quadX); i > 3; i--)
                    {
                        float y1 = 0;
                        float y2 = 0;
                        if (valueIndex > 0)
                        {
                            y2 = agent.actionsHistory[valueIndex] * scale;
                            y1 = agent.actionsHistory[valueIndex - 1] * scale;
                        }

                        GL.Vertex3(i, windowRect.height / 2 - y2, 0);
                        GL.Vertex3(i - 1, windowRect.height / 2 - y1, 0);
                        valueIndex -= 1;
                    }

                    GL.End();
                }

                GL.PopMatrix();
            }
        }
    }
}