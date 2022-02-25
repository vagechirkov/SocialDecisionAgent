using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SocialDecisionAgent.Runtime.Task.ColorMatching;
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

        [SerializeField] int speed = 5;
        
        // The list of pixels in the array of pixel rows (starting from the center of the square)
        List<ColorMatchingSquare>[] _squareRows;
        
        readonly Color32 _blue = new Color32(0, 0, 255, 255);
        readonly Color32 _orange = new Color32(255, 128, 0, 255);

        // Create the task
        void Awake()
        {
            var parentPosition = transform.position;
            
            // size of the array is equal to the number of half pixels in the row plus one
            _squareRows = new List<ColorMatchingSquare>[nPixelsHalf + 1];

            for (var i = -nPixelsHalf; i < nPixelsHalf; i++)
            for (var j = -nPixelsHalf; j < nPixelsHalf; j++)
            {
                var square = Instantiate(squarePrefab, transform);
                var squareScript = square.GetComponent<ColorMatchingSquare>();

                var width = squareScript.width;
                var squarePositionY = parentPosition.y + width * i + width / 2;
                var squarePositionZ = parentPosition.z + width * j + width / 2;

                square.transform.localPosition = new Vector3(0, squarePositionY, squarePositionZ);
                
                var row = Mathf.Max(Mathf.Abs(i), Mathf.Abs(j)); // distance from the center
                if (_squareRows[row] == null)
                    _squareRows[row] = new List<ColorMatchingSquare> {squareScript};
                else
                    _squareRows[row].Add(squareScript);
            }
        }


        public void GenerateSample()
        {
            StartCoroutine(DrawSquareRows(speed));
        }

        // Update the task with the `speed` rows per second
        IEnumerator DrawSquareRows(int rowPerSecond)
        {
            foreach (var squares in _squareRows)
            {
                foreach (var square in squares)
                {
                    // Coherence values are -1 = orange, 1 = blue
                    var color = Random.value > (Coherence + 1) / 2 ? _orange : _blue;
                    square.SetColor(color);
                }
                yield return new WaitForSeconds(rowPerSecond / 60f);
            }

            ResetColor();
        }

        public void ResetColor()
        {
            foreach (var square in _squareRows.SelectMany(s => s)) square.SetColor(Color.white);
        }
        
    }
}