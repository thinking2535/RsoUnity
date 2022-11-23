using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace rso.unity
{
    public class Dialog : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] Canvas _canvas;
        [SerializeField] Image _barrierDismissibleImage;
        [SerializeField] public bool barrierDismissible = true;
        public Canvas canvas { get => _canvas; }
        public float thickness = 0.0f;
        protected virtual void Awake()
        {
            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        }
        protected virtual void Update()
        {
        }
        protected virtual void OnDestroy()
        {
        }
        public virtual Task backButtonPressed()
        {
            return Task.CompletedTask;
        }
        public async void OnPointerDown(PointerEventData eventData)
        {
            if (_barrierDismissibleImage == null ||
                eventData.pointerPressRaycast.gameObject != _barrierDismissibleImage.gameObject ||
                !barrierDismissible)
                return;

            await backButtonPressed();
        }
    }
}