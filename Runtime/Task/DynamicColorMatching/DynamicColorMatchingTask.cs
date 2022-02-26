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

        [Tooltip("The number of pixel rows revealed per second")]
        [SerializeField] int speed = 30;
        
        readonly Color32 _blue = new Color32(0, 0, 255, 255);
        readonly Color32 _orange = new Color32(255, 128, 0, 255);
        
        
        [SerializeField] Image image;

        Texture2D _texture2D;
        List<Color> _trialSample;
        int[] _trialSampleRows;

        void Start()
        {
            _texture2D = new Texture2D(nPixelsHalf*2, nPixelsHalf*2);
        }

        public void GenerateSample()
        {
            _trialSample = CreateTrial(nPixelsHalf);
            _trialSampleRows = CreateTrialRows(nPixelsHalf);
            StartCoroutine(DrawSquareRows(speed, nPixelsHalf));
        }
        
        // Update the task with the `speed` rows per second
        IEnumerator DrawSquareRows(int rowPerSecond, int nPixels)
        {
            var t = Time.time;
            var cm = Enumerable.Repeat(Color.white, (nPixels * 2) * (nPixels * 2)).ToArray();
            for (var i = 0; i < nPixels + 1; i ++)
            {
                cm = cm.Select((val, inx) => _trialSampleRows[inx] == i ? _trialSample[inx] : val).ToArray();
                ApplyTexture(cm, _texture2D);
                yield return new WaitForSeconds(1f / rowPerSecond);
            }
            Debug.Log(Time.time - t);
        }
        
        List<Color> CreateTrial(int nPixels)
        {
            var colors = new List<Color>();
            for (var i = 0; i < (nPixels * 2) * (nPixels * 2); i++)
            {
                colors.Add(Random.value > (Coherence + 1) / 2 ? _orange : _blue);
            }

            return colors;
        }

        int[] CreateTrialRows(int nPixels)
        {
            var indices = new int[(nPixels * 2) * (nPixels * 2)];
            for (var i = -nPixels; i < nPixels; i++)
            for (var j = -nPixels; j < nPixels; j++)
            {
                var row = Mathf.Max(Mathf.Abs(i), Mathf.Abs(j)); // distance from the center
                var loc = (i + nPixels) * (nPixels * 2) + (j + nPixels);
                indices[loc] = row;
            }

            return indices;
        }

        void ApplyTexture(Color[] colorMap, Texture2D texture)
        {
            texture.SetPixels(colorMap);

            //Remove the blur from the texture
            texture.filterMode = FilterMode.Point;

            texture.wrapMode = TextureWrapMode.Clamp;

            texture.Apply();

            //Add the texture to the material
            image.overrideSprite = Sprite.Create(texture, 
                new Rect(0, 0, texture.width, texture.height), 
                new Vector2(0.5f, 0.5f));
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                GenerateSample();
            }
        }
        
    }
}