using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Selectable;

namespace rso.unity
{
    public class TextButton : ImageButton
    {
        [SerializeField] Text _text;
        public string text
        {
            get => _text.text;
            set => _text.text = value;
        }
        public Text getTextComponent()
        {
            return _text;
        }
    }
}