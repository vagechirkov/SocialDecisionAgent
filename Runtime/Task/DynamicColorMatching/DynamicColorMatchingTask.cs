using System;
using System.Collections;
using System.Collections.Generic;
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

        readonly Color32 _blue = new Color32(0, 0, 255, 255);
        readonly Color32 _orange = new Color32(255, 128, 0, 255);


        [SerializeField] Image image;

        Texture2D _texture2D;
        List<Color> _trialSample;
        int[] _trialSampleRows;
        int _nPixelsSquare;
        // The number of FixedDeltaTimes in one task
        float _nFixedDeltaTimes;
        float _nRowsPerFixedDeltaTimes;
        int _nFixedDeltaTimesPerRow;
        // The number of fixed updated to add throughout the task presentation
        int _nSparedFixedUpdates;

        void Start()
        {
            _texture2D = new Texture2D(nPixelsHalf * 2, nPixelsHalf * 2);
            _nPixelsSquare = nPixelsHalf * 2 * nPixelsHalf * 2;
            
            _nFixedDeltaTimes = doneInSeconds / Time.fixedDeltaTime;
            _nRowsPerFixedDeltaTimes = nPixelsHalf / _nFixedDeltaTimes;
            // (int)Math.Ceiling(1 / _nRowsPerFixedDeltaTimes);
            _nFixedDeltaTimesPerRow = (int)(1 / _nRowsPerFixedDeltaTimes); 
            _nFixedDeltaTimesPerRow = _nFixedDeltaTimesPerRow < 1 ? 1 : _nFixedDeltaTimesPerRow;

            _nSparedFixedUpdates = (int)_nFixedDeltaTimes - _nFixedDeltaTimesPerRow * nPixelsHalf;
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
            IsRunning = true;
            var startTime = Time.time;
            var cm = Enumerable.Repeat(Color.white, _nPixelsSquare).ToArray();

            var additionalWaiting = _nSparedFixedUpdates;
            var nExtraFixedUpdatePerRow = _nFixedDeltaTimesPerRow;
            for (var i = 0; i <= nPixelsHalf; i += NumberOfRowsPerDeltaTime)
            {
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
                    if (j!= nExtraFixedUpdatePerRow - 1)
                    {
                        yield return new WaitForFixedUpdate();
                        continue;
                    }
                    
                    cm = cm.Select((val, inx) => _trialSampleRows[inx] <= i ? _trialSample[inx] : val).ToArray();
                    ApplyTexture(cm, _texture2D);
                    
                    PercentageShown = (float) (2 * i) * (2 * i) / (2 * 2 * nPixelsHalf * nPixelsHalf) ;
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
            var cm = Enumerable.Repeat(Color.white, _nPixelsSquare).ToArray();
            ApplyTexture(cm, _texture2D);
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