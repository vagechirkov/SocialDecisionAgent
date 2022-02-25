using System.Collections.Generic;
using UnityEngine;

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
        
        [SerializeField] GameObject squarePrefab;
        
        [SerializeField] int nPixelsHalf = 64;
        
        readonly List<ColorMatching.ColorMatchingSquare> _squareScripts = new List<ColorMatching.ColorMatchingSquare>();
        
        readonly Color32 _blue = new Color32(0, 0, 255, 255);
        readonly Color32 _orange = new Color32(255, 128, 0, 255);

        // Create the task
        void Awake()
        {
            var parentPosition = transform.position;

            for (var i = -nPixelsHalf; i < nPixelsHalf; i++)
            for (var j = -nPixelsHalf; j < nPixelsHalf; j++)
            {
                var square = Instantiate(squarePrefab, transform);
                var squareScript = square.GetComponent<ColorMatching.ColorMatchingSquare>();

                var width = squareScript.width;
                var squarePositionY = parentPosition.y + width * i + width / 2;
                var squarePositionZ = parentPosition.z + width * j + width / 2;

                square.transform.localPosition = new Vector3(0, squarePositionY, squarePositionZ);
                
                _squareScripts.Add(squareScript);
            }
        }


        public void GenerateSample()
        {
            foreach (var square in _squareScripts) square.SetColor(Random.value > Coherence ? _orange : _blue);
        }
        
        public void ResetColor()
        {
            foreach (var square in _squareScripts) square.SetColor(Color.black);
        }
        
    }
}