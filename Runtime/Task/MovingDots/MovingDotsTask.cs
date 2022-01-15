using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SocialDecisionAgent.Runtime.Task.MovingDots
{
    public class MovingDotsTask : MonoBehaviour
    {
        [SerializeField] GameObject dotPrefab;
        [SerializeField] int numberOfDots = 50;
        [SerializeField] float radius = 3f;
        [SerializeField] float dotSpeed = 1f;
        [SerializeField] bool fixedCoherence = false;
        [SerializeField] float coherence = 0.5f;
        

        List<GameObject> _dots = new List<GameObject>();
        List<MovingDot> _dotScripts = new List<MovingDot>();
        TMP_Text _text;

        void OnEnable()
        {
            for (var i = 0; i < numberOfDots; i++)
            {
                var dot = Instantiate(dotPrefab,  transform);
                var dotScript = dot.GetComponent<MovingDot>();
                dotScript.speed = dotSpeed;
                dotScript.radius = radius;
                dotScript.areaCenterPosition = transform.localPosition;
                _dots.Add(dot);
                _dotScripts.Add(dotScript);
            }
            
            _text = GetComponentInChildren<TMP_Text>();
            
            GenerateMovingDots(coherence);

        }
        
        // <summary>
        // Generates the moving dots based on coherence value.
        // </summary>
        void GenerateMovingDots(float coherenceValue)
        {
            for (var i = 0; i < numberOfDots; i++)
            {
                var dot = _dots[i];
                var dotScript = _dotScripts[i];
                dot.SetActive(false);
                dotScript.direction = Random.value < Math.Abs(coherenceValue) ? - Math.Sign(coherenceValue) : 0f;
                dotScript.ResetMovingDirection();
                dot.SetActive(true);
            }
        }

        // <summary>
        // Generate coherence and random moving dots.
        // </summary>
        public float GenerateMovingDotsTrial()
        {
            if (!fixedCoherence)
                coherence = Random.value * 2 - 1f;
            else
                coherence *= Math.Sign(Random.value - 0.5f);

            GenerateMovingDots(coherence);
            _text.text = "Coherence: " + coherence.ToString("0.00");
            return coherence;
        }

    }
}
