using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [Tooltip("Time to show all rows")] [SerializeField]
        float doneInSeconds = 2f;

        readonly Color32 _blue = new Color32(0, 0, 255, 255);
        readonly Color32 _orange = new Color32(255, 128, 0, 255);


        [SerializeField] Image image;

        Texture2D _texture2D;
        List<Color> _trialSample;
        int[] _trialSampleRows;
        int _nPixelsSquare;

        void Start()
        {
            _texture2D = new Texture2D(nPixelsHalf * 2, nPixelsHalf * 2);
            _nPixelsSquare = nPixelsHalf * 2 * nPixelsHalf * 2;
        }

        public void GenerateSample()
        {
            _trialSample = CreateTrial();
            _trialSampleRows = CreateTrialRows();
            StartCoroutine(DrawSquareRows());
        }

        // Update the task with the `speed` rows per second
        // Note the it is slower for smaller FPS
        IEnumerator DrawSquareRows()
        {
            //var startTime = Time.time;
            var cm = Enumerable.Repeat(Color.white, _nPixelsSquare).ToArray();
            var waitTime = doneInSeconds / nPixelsHalf;
            var nRowsOneStep = 1;

            // Adjust speed to frame rate
            while (waitTime <= Time.deltaTime * 1.5f)
            {
                nRowsOneStep++;
                waitTime *= 2;
            }

            for (var i = 0; i <= nPixelsHalf; i += nRowsOneStep)
            {
                cm = cm.Select((val, inx) => _trialSampleRows[inx] <= i ? _trialSample[inx] : val).ToArray();
                ApplyTexture(cm, _texture2D);
                yield return new WaitForSeconds(waitTime);
            }
            //Debug.Log("Done in " + (Time.time - startTime) + " seconds");
        }

        // Fill the texture based on the coherence value
        List<Color> CreateTrial()
        {
            var colors = new List<Color>();
            for (var i = 0; i < _nPixelsSquare; i++)
                colors.Add(Random.value > (Coherence + 1) / 2 ? _orange : _blue);
            return colors;
        }

        // Create an array of rows stored in the flatten format to make a dynamic color matching task
        int[] CreateTrialRows()
        {
            var indices = new int[_nPixelsSquare];
            for (var i = -nPixelsHalf; i < nPixelsHalf; i++)
            for (var j = -nPixelsHalf; j < nPixelsHalf; j++)
            {
                var row = Mathf.Max(Mathf.Abs(i), Mathf.Abs(j)); // distance from the center
                var loc = (i + nPixelsHalf) * (nPixelsHalf * 2) + j + nPixelsHalf;
                indices[loc] = row;
            }

            return indices;
        }

        // Apply the texture to the image
        void ApplyTexture(Color[] colorMap, Texture2D texture)
        {
            texture.SetPixels(colorMap);

            //Remove the blur from the texture
            texture.filterMode = FilterMode.Point;

            texture.wrapMode = TextureWrapMode.Clamp;

            texture.Apply();

            //Add the texture as a sprite
            image.overrideSprite = Sprite.Create(texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
        }
    }
}