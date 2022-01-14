using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SDM.Task.ColorMatching
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
