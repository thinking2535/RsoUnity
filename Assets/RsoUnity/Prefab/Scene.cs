using UnityEngine;

namespace rso.unity
{
    public class Scene : MonoBehaviour
    {
        public new Camera camera { get; private set; }
        protected virtual void Awake()
        {
            camera = GetComponentInChildren<Camera>();
        }
        protected virtual void Start()
        {
        }
        protected virtual void Update()
        {
        }
        protected virtual void OnDestroy()
        {
        }
    }
}