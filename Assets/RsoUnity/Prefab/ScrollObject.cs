using rso.math;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace rso.unity
{
    public class ScrollObject : InputObject, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public delegate void FCallback(InputType inputType, Vector2 downPosition, Vector2 diff, Vector2 delta);

        public FCallback fCallback;
        Vector2 _downPosition;
        Vector2 _lastPosition;

        public void OnPointerDown(PointerEventData eventData)
        {
            _downPosition = _lastPosition = eventData.position;
            fCallback(InputType.down, _downPosition, Vector2.zero, Vector2.zero);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            fCallback(InputType.up, _downPosition, eventData.position - _downPosition, eventData.position - _lastPosition);
        }
        public void OnDrag(PointerEventData eventData)
        {
            fCallback(InputType.move, _downPosition, eventData.position - _downPosition, eventData.position - _lastPosition);
            _lastPosition = eventData.position;
        }
    }
}