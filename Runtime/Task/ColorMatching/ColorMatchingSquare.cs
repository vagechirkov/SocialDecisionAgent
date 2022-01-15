using UnityEngine;

namespace SocialDecisionAgent.Runtime.Task.ColorMatching
{
    public class ColorMatchingSquare : MonoBehaviour
    {
        [SerializeField] public float width = 0.1f;

        SpriteRenderer SprRend { get; set; }


        void OnEnable()
        {
            SprRend = GetComponentInChildren<SpriteRenderer>();
            SprRend.size = new Vector2(width, width);
            SprRend.color = Random.ColorHSV();
        }
    }
}