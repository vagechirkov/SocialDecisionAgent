using System.Collections.Generic;
using SocialDecisionAgent.Runtime.Task.ColorMatching;
using UnityEngine;
using UnityEngine.UI;

namespace SocialDecisionAgent.Runtime.Task.DynamicColorMatching
{
    /// <summary>
    ///     This class is responsible for the dynamic color classification task.
    ///     For more information about the task, please refer to the paper:
    ///     Voss, A., Rothermund, K., & Brandtstädter, J. (2008). Interpreting ambiguous stimuli: Separating perceptual
    ///     and judgmental biases. Journal of Experimental Social Psychology, 44(4), 1048–1056.
    ///     https://doi.org/10.1016/j.jesp.2007.10.009
    /// </summary>
    public class DynamicColorMatchingTask : MonoBehaviour, ITask
    {
        // -1 = orange, 1 = blue
        public float Coherence { get; set; } = 0f;

        [SerializeField] int nPixelsHalf = 64;

        [Tooltip("The number of pixel rows revealed per second")]
        [SerializeField] int speed = 5;
        
        // The list of pixels in the array of pixel rows (starting from the center of the square)
        List<ColorMatchingSquare>[] _squareRows;
        
        readonly Color32 _blue = new Color32(0, 0, 255, 255);
        readonly Color32 _orange = new Color32(255, 128, 0, 255);
        
        Vector3[] vertices;
        
        public void GenerateSample()
        {
            vertices = new Vector3[(nPixelsHalf + 1) * (nPixelsHalf + 1)];
            for (int i = 0, y = 0; y <= nPixelsHalf; y++) {
                for (int x = 0; x <= nPixelsHalf; x++, i++) {
                    vertices[i] = new Vector3(0, x / 10f - 1f, y / 10f - 1f);
                }
            }
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                GenerateSample();
            }
        }
        
        void OnDrawGizmos () {
            if (vertices == null) {
                return;
            }
            Gizmos.color = Color.black;
            for (int i = 0; i < vertices.Length; i++) {
                Gizmos.DrawSphere(vertices[i], 0.1f);
            }
        }
    }
}