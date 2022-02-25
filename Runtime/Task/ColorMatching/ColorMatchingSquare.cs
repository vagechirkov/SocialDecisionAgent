using UnityEngine;

namespace SocialDecisionAgent.Runtime.Task.ColorMatching
{
    public class ColorMatchingSquare : MonoBehaviour
    {
        [SerializeField] public float width = 0.1f;

        SpriteRenderer SprRend { get; set; }
        
        void Awake()
        {
            SprRend = GetComponentInChildren<SpriteRenderer>();
            SprRend.size = new Vector2(width, width);
            SprRend.color = Color.white;
        }
        
        public void SetColor(Color color)
        {
            SprRend.color = color;
        }
    }
}