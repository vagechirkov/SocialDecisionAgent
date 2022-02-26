using System;
using System.Collections;
using System.Collections.Generic;
using SocialDecisionAgent.Runtime.Task.ColorMatching;
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

        [SerializeField] int nPixelsHalf = 64;

        [Tooltip("The number of pixel rows revealed per second")]
        [SerializeField] int speed = 30;
        
        readonly Color32 _blue = new Color32(0, 0, 255, 255);
        readonly Color32 _orange = new Color32(255, 128, 0, 255);
        
        
        [SerializeField] Image image;

        Texture2D _texture2D;

        void Start()
        {
            _texture2D = new Texture2D(nPixelsHalf*2, nPixelsHalf*2);
        }

        public void GenerateSample()
        {
            StartCoroutine(DrawSquareRows(speed));
        }
        
        // Update the task with the `speed` rows per second
        IEnumerator DrawSquareRows(int rowPerSecond)
        {
            var colorMaps = CreateColorMaps(nPixelsHalf);
            var t = Time.time;
            foreach (var cm in colorMaps)
            {
                ApplyTexture(cm, _texture2D);
                yield return new WaitForSeconds(1f / rowPerSecond);
            }
            Debug.Log(Time.time - t);
        }

        IEnumerable<Color[]> CreateColorMaps(int nPixels)
        {
            var colorMaps = new Color[nPixels][];

            var width = nPixels * 2;
            var height = nPixels * 2;

            for (var iRow = 0; iRow < colorMaps.Length; iRow++)
            {
                var colorMap = new Color[width * height];
                for (var i = 0; i < height; i++)
                {
                    for (var j = 0; j < width; j++)
                    {
                        var row = Mathf.Max(Mathf.Abs(i - nPixels), Mathf.Abs(j - nPixels));
                        if (row <= iRow)
                            colorMap[i * height + j] = Random.value > (Coherence + 1) / 2 ? _orange : _blue;
                        else
                            colorMap[i * height + j] = Color.white;
                    }
                }

                colorMaps[iRow] = colorMap;
            }

            return colorMaps;
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