using rso.math;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace rso.unity
{
    public class JoypadObject : InputObject, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public delegate void FCallback(InputType inputType, Int32 direction);

        public FCallback fCallback = null;
        float _activeRange;
        Int32 _directionCount = 1;
        float _unitTheta;
        float _unitTheta_2;
        Vector2 _defaultPosition;
        public Vector2 centerPosition { get; private set; }
        public Vector2 touchPosition { get; private set; }
        public Vector2 relativeTouchPosition { get => touchPosition - centerPosition; }
        Int32 _lastDirection = -1; // 9시 방향부터 0 ~ 반시계방향으로

        public void init(float activeRange, Int32 expDirectionCount, Vector2 defaultPosition)
        {
            if (activeRange < 0.0f)
                throw new Exception("Invalid ActiveRange");

            if (expDirectionCount < 0)
                throw new Exception("Invalid ExpDirCount Count");

            _activeRange = activeRange;

            for (Int32 i = 0; i < expDirectionCount; ++i)
                _directionCount *= 2;

            _unitTheta = Mathf.PI * 2.0f / _directionCount;
            _unitTheta_2 = _unitTheta * 0.5f;
            touchPosition = centerPosition = _defaultPosition = defaultPosition;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            touchPosition = centerPosition = eventData.position;
            fCallback(InputType.down, -1);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            _lastDirection = -1;
            touchPosition = centerPosition = _defaultPosition;
            fCallback(InputType.up, -1);
        }
        public void OnDrag(PointerEventData eventData)
        {
            touchPosition = eventData.position;
            var vector = touchPosition - centerPosition;
            var magnitude = vector.magnitude;
            if (_lastDirection == -1)
            {
                if (magnitude < _activeRange)
                    return;
            }

            var theta = Mathf.PI - Mathf.Atan2(vector.y, vector.x);
            theta += _unitTheta_2;
            theta %= CMath.c_2_PI_F;
            var direction = (Int32)(theta / _unitTheta);

            if (magnitude > _activeRange)
            {
                vector.x = vector.x / magnitude * _activeRange;
                vector.y = vector.y / magnitude * _activeRange;
                centerPosition = touchPosition - vector;
            }

            if (direction != _lastDirection)
                fCallback(InputType.move, direction);

            _lastDirection = direction;
        }
    }
}