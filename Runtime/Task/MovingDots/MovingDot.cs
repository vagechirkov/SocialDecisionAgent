using System;
using UnityEngine;
using Random = System.Random;


namespace SDM.Task.MovingDots
{
    public class MovingDot : MonoBehaviour
    {

        [HideInInspector] public float speed = 1.0f;
        [HideInInspector] public float radius = 2.0f;
        [HideInInspector] public float direction = 0.0f;
        [HideInInspector] public Vector3 areaCenterPosition;
        
        readonly Random _random = new Random(Environment.TickCount);
        
        Vector3 _movingDirection;
        float _lifeTime;

        void OnEnable()
        {
            transform.localPosition = new Vector3(0,
                (float)(_random.NextDouble() * 1.8 - 1 ) * radius,
                (float)(_random.NextDouble() * 1.8 - 1) * radius);
            UpdateMovingDirection();
            _lifeTime = (float) _random.NextDouble() * 250;
        }
        
        void Update()
        {
            UpdatePosition();
            transform.localPosition += _movingDirection * speed * Time.deltaTime;
            UpdateTimer();
        }
        
        public void UpdateMovingDirection()
        {
            if (direction != 0)
                _movingDirection = new Vector3(0, 0, (float)(_random.NextDouble() + 0.2) * direction);
            else
                _movingDirection = new Vector3(0, (float)(_random.NextDouble() - 0.5), (float)(_random.NextDouble() -0.5));
        }
        
        void UpdatePosition()
        {
            if ((transform.localPosition - areaCenterPosition).magnitude > radius)
            {
                transform.localPosition = new Vector3(0,
                    (float) (_random.NextDouble() * 1.8 - 1) * radius,
                    (float) (_random.NextDouble() * 1.8 - 1) * radius);
                UpdateMovingDirection();
            }
        }
        
        void UpdateTimer()
        {
            _lifeTime -= Time.deltaTime;
            if (_lifeTime < 0)
            {
                _lifeTime = (float) _random.NextDouble() * 250;
                UpdateMovingDirection();
                UpdatePosition();
            }
        }
    }
}