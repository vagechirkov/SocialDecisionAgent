using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace SDM.Task.MovingDots
{
    public class MovingDots : MonoBehaviour
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

        readonly Random _random = new Random(Environment.TickCount);

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
            
            for (var i = 0; i < 10; i++)
            {
                GenerateMovingDots(0.3f);
            }
        }

        public float GenerateMovingDotsTrial()
        {
            if (!fixedCoherence)
                coherence = (float) (_random.NextDouble() * 2 - 1f);
            else
                coherence *= Math.Sign(_random.NextDouble() - 0.5f);

            GenerateMovingDots(coherence);
            _text.text = "Coherence: " + coherence.ToString("0.00");
            return coherence;
        }

        void GenerateMovingDots(float coherenceValue)
        {
            for (var i = 0; i < numberOfDots; i++)
            {
                var dot = _dots[i];
                var dotScript = _dotScripts[i];
                dot.SetActive(false);
                dotScript.direction = _random.NextDouble() < Math.Abs(coherenceValue) ? - Math.Sign(coherenceValue) : 0f;
                dotScript.UpdateMovingDirection();
                dot.SetActive(true);
            }
        }

    }
}
