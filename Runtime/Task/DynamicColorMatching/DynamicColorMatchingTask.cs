using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

        public bool ResetAfterFinish { get; set; } = true;

        // Time that was taken to finish the whole task
        public float FinishedInSeconds { get; set; }

        // Is the task running?
        public bool IsRunning { get; set; }

        // Percentage of the task revealed
        public float PercentageShown { get; set; }

        // Time passed since the beginning
        public float TimePassed { get; set; }

        public int NumberOfRowsPerDeltaTime = 1;

        [SerializeField] int nPixelsHalf = 64;

        [Tooltip("Time to show all rows")] [SerializeField]
        float doneInSeconds = 2f;

        readonly Color _blue = new Color32(0, 0, 255, 255);
        readonly Color _orange = new Color32(255, 128, 0, 255);

        [SerializeField] Image image;

        Texture2D _texture2D;
        int[] _trialSampleRows;

        int _nPixelsSquare;

        // The number of FixedDeltaTimes in one task
        float _nFixedDeltaTimes;
        float _nRowsPerFixedDeltaTimes;

        int _nFixedDeltaTimesPerRow;

        // The number of fixed updated to add throughout the task presentation
        int _nSparedFixedUpdates;

        Color[] _cmWhite, _cmBlueFull, _cmOrangeHalf, _trialSample;

        void Start()
        {
            _texture2D = new Texture2D(nPixelsHalf * 2, nPixelsHalf * 2);
            _nPixelsSquare = nPixelsHalf * 2 * nPixelsHalf * 2;

            _nFixedDeltaTimes = doneInSeconds / Time.fixedDeltaTime;
            _nRowsPerFixedDeltaTimes = nPixelsHalf / _nFixedDeltaTimes;
            // (int)Math.Ceiling(1 / _nRowsPerFixedDeltaTimes);
            _nFixedDeltaTimesPerRow = (int) (1 / _nRowsPerFixedDeltaTimes);
            _nFixedDeltaTimesPerRow = _nFixedDeltaTimesPerRow < 1 ? 1 : _nFixedDeltaTimesPerRow;

            _nSparedFixedUpdates = (int) _nFixedDeltaTimes - _nFixedDeltaTimesPerRow * nPixelsHalf;

            // Create an array of rows stored in the flatten format to make a dynamic color matching task
            // can be done once per experiment
            _trialSampleRows = CreateTrialRows();

            // Create the white color array
            _cmWhite = Enumerable.Repeat(Color.white, _nPixelsSquare).ToArray();

            // Create blue color array
            _cmBlueFull = Enumerable.Repeat(_blue, _nPixelsSquare).ToArray();

            // This can be useful when Coherence is 0
            _cmOrangeHalf = Enumerable.Repeat(_orange, _nPixelsSquare / 2).ToArray();
        }

        public void GenerateSample()
        {
            _trialSample = CreateTrial();
            StartCoroutine(DrawSquareRows());
        }

        // Update the task with the `speed` rows per second
        // Note the it is slower for smaller FPS
        IEnumerator DrawSquareRows()
        {
            IsRunning = true;
            var startTime = Time.time;
            // Create a white array
            var cm = new Color[_cmWhite.Length];
            _cmWhite.CopyTo(cm, 0);

            var additionalWaiting = _nSparedFixedUpdates;
            for (var i = 0; i <= nPixelsHalf; i += NumberOfRowsPerDeltaTime)
            {
                int nExtraFixedUpdatePerRow;
                if (additionalWaiting > 0)
                {
                    nExtraFixedUpdatePerRow = _nFixedDeltaTimesPerRow + 1;
                    additionalWaiting--;
                }
                else
                {
                    nExtraFixedUpdatePerRow = _nFixedDeltaTimesPerRow;
                }

                for (var j = 0; j < nExtraFixedUpdatePerRow; j++)
                {
                    if (j != nExtraFixedUpdatePerRow - 1)
                    {
                        yield return new WaitForFixedUpdate();
                        continue;
                    }

                    // Copy one row from the _trialSample array to the array cm (white)
                    for (var inx = 0; inx < _trialSampleRows.Length; inx++) 
                        if (_trialSampleRows[inx] <= i) cm[inx] = _trialSample[inx];
                    
                    ApplyTexture(cm, _texture2D);

                    PercentageShown = (float) (2 * i) * (2 * i) / (2 * 2 * nPixelsHalf * nPixelsHalf);
                    TimePassed = Time.time - startTime;

                    yield return new WaitForFixedUpdate();
                }
            }

            FinishedInSeconds = Time.time - startTime;
            IsRunning = false;

            if (!ResetAfterFinish) yield break;

            yield return new WaitForSeconds(0.1f);
            ResetSample();
        }

        public void ResetSample()
        {
            ApplyTexture(_cmWhite, _texture2D);
        }

        // Fill the texture based on the coherence value
        Color[] CreateTrial()
        {
            // colors array is filled with blue colors
            var colors = new Color[_cmBlueFull.Length];
            _cmBlueFull.CopyTo(colors, 0);

            // Add the orange colors depending on the coherence value
            if (Coherence == 0)
            {
                _cmOrangeHalf.CopyTo(colors, _cmOrangeHalf.Length);
            }
            else
            {
                // negative coherence is orange, positive is blue
                // Example: Coherence = -0.5 (orange) and _nPixelsSquare = 16384
                // nOrange = 16384 - 0.25 * 16384 = 12288
                var nOrange = (int) (_nPixelsSquare - (Coherence + 1) / 2 * _nPixelsSquare);
                // Loop here is inevitable
                for (var i = 0; i < nOrange; i++) colors[i] = _orange;
            }

            // NOTE: Expensive operation
            Randomize(colors);
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
                var loc = (i + nPixelsHalf) * nPixelsHalf * 2 + j + nPixelsHalf;
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
            image.overrideSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
        }
        
        // Randomize order of elements in array
        static void Randomize<T>(T[] items)
        {
            // For each spot in the array, pick
            // a random item to swap into that spot.
            for (var i = 0; i < items.Length - 1; i++)
            {
                var j = Random.Range(i, items.Length);
                (items[i], items[j]) = (items[j], items[i]);
            }
        }
    }
}