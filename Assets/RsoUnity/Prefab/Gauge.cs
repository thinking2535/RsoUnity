using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
    [SerializeField] Vector2 _minSize;
    [SerializeField] Vector2 _maxSize;
    [SerializeField] RectTransform _barRectTransform;
    [SerializeField] Text _text;

    Vector2 _range;

    public void init()
    {
        _range = _maxSize - _minSize;
    }
    public Vector2 size
    {
        get
        {
            return (_barRectTransform.sizeDelta - _minSize) / _range;
        }
        set
        {
            _barRectTransform.sizeDelta = _minSize + _range * value;
        }
    }
    public string text
    {
        get => _text.text;
        set => _text.text = value;
    }
    public void activeText(bool value)
    {
        _text.gameObject.SetActive(value);
    }
}
