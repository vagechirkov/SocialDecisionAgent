using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SocialDecisionAgent.Runtime.Task.ColorMatching
{
    public class ColorMatchingTask : MonoBehaviour, ITask
    {
        [SerializeField] GameObject squarePrefab;
        
        List<GameObject> _squares = new List<GameObject>();
        
        List<ColorMatchingSquare> _squareScripts = new List<ColorMatchingSquare>();
        
        public float Coherence { get; set; }
        
        void OnEnable()
        {
            var parentPosition = transform.position;
            
            for (var i = -20; i < 20; i++)
            {
                for (var j = -20; j < 20; j++)
                {
                    var square = Instantiate(squarePrefab,  transform);
                    var squareScript = square.GetComponent<ColorMatchingSquare>();
                    
                    var width = squareScript.width;
                    var squarePositionY = parentPosition.y + width*i + width/2;
                    var squarePositionZ = parentPosition.z + width*j + width/2;
                    
                    square.transform.position = new Vector3(0, squarePositionY, squarePositionZ);
                   
                    
                    _squares.Add(square);
                    _squareScripts.Add(squareScript);
                }
            }
        }


        public void GenerateSample()
        {
            for (var i = 0; i < 100; i++)
            {
                for (var j = 0; j < 100; j++)
                {
                    
                }
            }
            
            
        }
        
    }
}