using UnityEngine;
using Random = UnityEngine.Random;

namespace SocialDecisionAgent.Runtime.Task.MovingDots
{
    public class MovingDot : MonoBehaviour
    {
        public float speed = 1.0f;
        public float radius = 2.0f;
        public float direction;
        public Vector3 areaCenterPosition;

        Vector3 _movingDirection;

        void Awake()
        {
            ResetPosition();
            ResetMovingDirection();
        }

        void Update()
        {
            transform.localPosition += _movingDirection * speed * Time.deltaTime;
            if ((transform.localPosition - areaCenterPosition).magnitude > radius)
                ResetPosition();
        }

        void ResetPosition()
        {
            var newPosition = Random.insideUnitCircle * radius;
            transform.localPosition = new Vector3(0, newPosition.x, newPosition.y);
        }

        public void ResetMovingDirection()
        {
            if (direction != 0)
                _movingDirection = new Vector3(0, 0, direction);
            else
                _movingDirection = new Vector3(0, (Random.value - 0.5f) * 2, (Random.value - 0.5f) * 2);

            _movingDirection = Vector3.Normalize(_movingDirection);
        }
    }
}