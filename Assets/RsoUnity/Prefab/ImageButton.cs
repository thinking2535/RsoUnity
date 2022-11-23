using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Selectable;

namespace rso.unity
{
    public class ImageButton : EmptyButton
    {
        public Image image
        {
            get => _button.image;
            set => _button.image = value;
        }
    }
}