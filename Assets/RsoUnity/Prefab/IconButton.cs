using UnityEngine;
using UnityEngine.UI;

namespace rso.unity
{
    public class IconButton : TextButton
    {
        [SerializeField] Image _Icon;
        public Sprite icon
        {
            get => _Icon.sprite;
            set => _Icon.sprite = value;
        }

    }
}