using System.Collections.Generic;
using UnityEngine;

namespace SocialDecisionAgent.Runtime.Task.ColorMatching
{
    public class ColorMatchingTask : MonoBehaviour, ITask
    {
        [SerializeField] GameObject squarePrefab;

        [SerializeField] int nPixelsHalf = 20;

        [SerializeField] bool fixedCoherence;
        
        [SerializeField] float coherence = 0.5f;

        readonly List<GameObject> _squares = new List<GameObject>();

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

                square.transform.position = new Vector3(0, squarePositionY, squarePositionZ);

                _squares.Add(square);
                _squareScripts.Add(squareScript);
            }
        }


        public void GenerateSample()
        {
            if (!fixedCoherence)
                Coherence = Random.value;
            else
                Coherence = coherence;
            
            foreach (var square in _squareScripts) square.SetColor(Random.value > Coherence ? Color.red : Color.green);
        }
    }
}