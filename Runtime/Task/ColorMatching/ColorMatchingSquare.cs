using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace SocialDecisionAgent.Runtime.Task.ColorMatching
{

    public class ColorMatchingSquare : MonoBehaviour
    {
        SpriteRenderer _spriteRenderer;
        public float Width { get; private set; }

        void OnEnable()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            Width = _spriteRenderer.bounds.size.x;
            _spriteRenderer.color = Random.ColorHSV();
        }
    }

}
