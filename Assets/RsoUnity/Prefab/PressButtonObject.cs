using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace rso.unity
{
    public class PressButtonObject : InputObject, IPointerDownHandler
    {
        public delegate void FCallback();

        public FCallback fCallback = null;
        public void OnPointerDown(PointerEventData eventData)
        {
            fCallback?.Invoke();
        }
    }
}