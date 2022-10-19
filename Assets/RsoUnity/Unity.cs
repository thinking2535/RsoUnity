using rso.gameutil;
using rso.physics;
using UnityEditor;
using UnityEngine;

namespace rso.unity
{
    // ����Ƽ �߷� ���ӵ� ����
    // ���� = 0.5 * g * t*t + (����v + ����FixedUpdate������v) * 0.5 * t    ## (����v + ����FixedUpdate������v) * 0.5 = ����v�� ����v�� �߰�
    // ���� = 0.5 * t * (g *  t + ����v + ����FixedUpdate������v)

    public static class CUnity
    {
        public static EOS GetOS()
        {
#if UNITY_ANDROID
            return EOS.Android;
#elif UNITY_IOS
    		return EOS.iOS;
#else
            return EOS.iOS;
#endif
        }
        public static string GetDataPath()
        {
#if UNITY_EDITOR
            return Application.dataPath + "/";
#else
            return Application.persistentDataPath + "/";
#endif
        }
        public static void ApplicationQuit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
        public static void ApplicationPause()
        {
#if UNITY_EDITOR
            EditorApplication.isPaused = true;
#else
            Application.Quit();
#endif
        }
        public static bool IsKeyDown(KeyCode KeyCode_)
        {
#if UNITY_ANDROID || UNITY_EDITOR
            return Input.GetKeyDown(KeyCode_);
#else
            return false;
#endif
        }
    }
}