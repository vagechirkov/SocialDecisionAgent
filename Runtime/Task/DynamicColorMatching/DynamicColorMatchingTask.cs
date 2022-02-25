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
        
        [SerializeField] Material imageMaterial;
        
        [SerializeField] Image image;
        
        public void GenerateSample()
        {
            //From random values to colors
            Color[] colorMap = new Color[nPixelsHalf * nPixelsHalf];

            for (int x = 0; x < nPixelsHalf * nPixelsHalf; x++)
            {
                //The colors are gray scale
                colorMap[x] = Color.Lerp(Color.black, Color.white, Random.value);
            }

            //Add the colors to the texture
            Texture2D texture = new Texture2D(nPixelsHalf, nPixelsHalf);

            texture.SetPixels(colorMap);

            //Remove the blur from the texture
            texture.filterMode = FilterMode.Point;

            texture.wrapMode = TextureWrapMode.Clamp;

            texture.Apply();

            //Add the texture to the material
            image.overrideSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
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