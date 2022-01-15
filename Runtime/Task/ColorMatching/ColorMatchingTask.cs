using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SocialDecisionAgent.Runtime.Task.ColorMatching
{
    public class ColorMatchingTask : MonoBehaviour
    {
        [SerializeField] GameObject squarePrefab;


        List<GameObject> _squares = new List<GameObject>();
        List<ColorMatchingSquare> _squareScripts = new List<ColorMatchingSquare>();


        void OnEnable()
        {
            
            for (var i = -20; i < 20; i++)
            {
                for (var j = -20; j < 20; j++)
                {
                    var square = Instantiate(squarePrefab,  transform);
                    var position = transform.position;
                    square.transform.position = new Vector3(0, position.y + (float) i / 5, position.z +(float) j / 5);
                    var squareScript = square.GetComponent<ColorMatchingSquare>();
                    
                    _squares.Add(square);
                    _squareScripts.Add(squareScript);
                }
            }
        }
        

        void GenerateSample()
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