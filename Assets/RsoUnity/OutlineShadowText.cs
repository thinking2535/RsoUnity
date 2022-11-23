// MenuItem에 등록된 스크립트가 프리팹에 사용되었고, 이후 스크립트를 수정할 경우 해당 프리팹의 나머디 SerializeField 가 모두 null 이 되어버리는데
// 이유가 뭘까?

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using bb;
//using System.Threading.Tasks;

//#if UNITY_EDITOR
//using UnityEditor;

//namespace rso.unity
//{
//    class OutlineShadowText
//    {
//        [MenuItem("GameObject/UI/OutlineShadowText")]
//        static void CreateOutlineShadowText()
//        {
//            Canvas parentCanvas;
//            if (Selection.activeGameObject != null)
//                parentCanvas = Selection.activeGameObject.GetComponentInParent<Canvas>();
//            else
//                parentCanvas = GameObject.FindObjectOfType<Canvas>();

//            GameObject textGameObject = new GameObject("Text");
//            textGameObject.AddComponent<CanvasRenderer>();
//            var text = textGameObject.AddComponent<Text>();
//            text.resizeTextForBestFit = true;
//            text.alignment = TextAnchor.MiddleCenter;
//            text.resizeTextMinSize = 0;
//            text.rectTransform.sizeDelta = new Vector2(160.0f, 30.0f);
//            text.fontSize = 36;
//            text.text = "New Text";

//            var outline = textGameObject.AddComponent<Outline>();
//            outline.effectColor = new Color(0.22f, 0.22f, 0.22f, 1.0f);

//            var shadow = textGameObject.AddComponent<Shadow>();
//            shadow.effectColor = new Color(0.22f, 0.22f, 0.22f, 0.8f);

//            GameObject addedRootGameObject = null;
//            if (parentCanvas != null)
//            {
//                if (Selection.activeGameObject != null)
//                    textGameObject.transform.SetParent(Selection.activeGameObject.transform, false);
//                else
//                    textGameObject.transform.SetParent(parentCanvas.gameObject.transform, false);
//            }
//            else
//            {
//                var canvasGameObject = new GameObject("Canvas");

//                var canvas = canvasGameObject.AddComponent<Canvas>();
//                canvas.renderMode = RenderMode.ScreenSpaceOverlay;

//                canvasGameObject.AddComponent<CanvasScaler>();
//                canvasGameObject.AddComponent<GraphicRaycaster>();

//                textGameObject.transform.SetParent(canvasGameObject.transform, false);

//                if (Selection.activeGameObject != null)
//                    canvasGameObject.transform.SetParent(Selection.activeGameObject.transform, false);

//                addedRootGameObject = canvasGameObject;
//            }

//            if (addedRootGameObject == null)
//                addedRootGameObject = textGameObject;

//            Selection.activeObject = textGameObject;
//            Undo.RegisterCreatedObjectUndo(addedRootGameObject, "Create OutlineShadowText");
//        }
//    }
//}
//#endif


//// 아래 클래스는 새로운 컴포넌트 를 등록하기 위한 클래스
//#if UNITY_EDITOR
//[CustomEditor(typeof(OutlineShadowText))]
//public class OutlineShadowTextEditor : Editor
//{
//    OutlineShadowText targetObject;
//    void OnEnable()
//    {
//        targetObject = (OutlineShadowText)target;
//    }
//    public override void OnInspectorGUI()
//    {
//        EditorGUI.BeginChangeCheck();

//        var oldFontSize = targetObject.fontSize;

//        base.OnInspectorGUI();

//        if (EditorGUI.EndChangeCheck())
//        {
//            if (targetObject.fontSize != oldFontSize)
//                targetObject.Update();
//        }
//    }
//}
//#endif
//public class OutlineShadowText : Text
//{
//}
