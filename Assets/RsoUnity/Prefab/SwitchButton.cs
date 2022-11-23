using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace rso.unity
{
    public class SwitchButton : MonoBehaviour
    {
        [SerializeField] bool _value = true;
        [SerializeField] public Text title;
        [SerializeField] public Text onText;
        [SerializeField] public Text offText;
        [SerializeField] Image _onImage;
        [SerializeField] Image _offImage;
        [SerializeField] Button _button;

        public Action<bool> onToggled = null;

        void Awake()
        {
            _button.onClick.AddListener(toggle);
            _update();
        }
        void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
        public void toggle()
        {
            _value = !_value;
            _update();
            onToggled?.Invoke(_value);
        }
        public void setValue(bool value)
        {
            if (value == _value)
                return;

            toggle();
        }
        public bool getValue()
        {
            return _value;
        }
        void _update()
        {
            _onImage.gameObject.SetActive(_value);
            _offImage.gameObject.SetActive(!_value);
        }
    }
}