using UnityEngine;
using UnityEngine.UI;

namespace SocialDecisionAgent.Runtime.Task.DynamicColorMatching
{
    public class DynamicColorMatchingTexture : MonoBehaviour
    {
        [SerializeField] Image image;
        
        Texture2D texture = new Texture2D(128, 128);   
        
        void Start()
        {
            
            image.material.mainTexture = texture;
            
            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    Color color = ((x & y) > Random.value * 100 ? Color.white : Color.gray);
                    texture.SetPixel(x, y, color);
                }
            }
            texture.Apply();
        }
    }
}
