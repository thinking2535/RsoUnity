using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Selectable;

namespace rso.unity
{
    public class EmptyButton : MonoBehaviour
    {
        [SerializeField] protected Button _button;
        public void AddListener(UnityAction call)
        {
            _button.onClick.AddListener(call);
        }
        void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}