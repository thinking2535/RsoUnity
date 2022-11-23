using UnityEngine;

namespace rso.unity
{
    public abstract class InputObject : MonoBehaviour
    {
        public enum InputType
        {
            down,
            move,
            up,
            cancel
        }
    }
}