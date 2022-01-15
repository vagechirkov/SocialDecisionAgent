using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SocialDecisionAgent.Runtime.Task.ColorMatching
{

    public class ColorMatchingSquare : MonoBehaviour
    {
        SpriteRenderer _spriteRenderer;

        void OnEnable()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

}
