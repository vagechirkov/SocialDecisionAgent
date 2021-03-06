using System.Collections.Generic;
using UnityEngine;

namespace SocialDecisionAgent.Runtime.Task.ColorMatching
{
    public class ColorMatchingTask : MonoBehaviour, ITask
    {
        [SerializeField] GameObject squarePrefab;

        [SerializeField] int nPixelsHalf = 20;

        [SerializeField] bool fixedCoherence;
        
        [SerializeField] public float coherence = 0.5f;

        readonly List<ColorMatchingSquare> _squareScripts = new List<ColorMatchingSquare>();

        public float Coherence { get; set; }

        void Awake()
        {
            var parentPosition = transform.position;

            for (var i = -nPixelsHalf; i < nPixelsHalf; i++)
            for (var j = -nPixelsHalf; j < nPixelsHalf; j++)
            {
                var square = Instantiate(squarePrefab, transform);
                var squareScript = square.GetComponent<ColorMatchingSquare>();

                var width = squareScript.width;
                var squarePositionY = parentPosition.y + width * i + width / 2;
                var squarePositionZ = parentPosition.z + width * j + width / 2;

                square.transform.localPosition = new Vector3(0, squarePositionY, squarePositionZ);
                
                _squareScripts.Add(squareScript);
            }
        }


        public void GenerateSample()
        {
            Coherence = !fixedCoherence ? Random.value : coherence;

            foreach (var square in _squareScripts) square.SetColor(Random.value > Coherence ? Color.red : Color.green);
        }
        
        public void ResetColor()
        {
            foreach (var square in _squareScripts) square.SetColor(Color.blue);
        }
        
    }
}