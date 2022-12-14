using rso.unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace rso.unity
{
    public class ConsoleWindow : MonoBehaviour
    {
        public delegate void FInput(string Text_);

        [SerializeField] Canvas _Canvas;
        [SerializeField] GameObject _InputBar;
        [SerializeField] GameObject _Body;
        [SerializeField] InputField _Input;
        [SerializeField] Button _FoldUnfoldButton;
        [SerializeField] Button _ClearButton;
        [SerializeField] Button _CloseButton;
        [SerializeField] Button _InputButton;
        [SerializeField] Text _Content;
        [SerializeField] ScrollRect _ContentScrollRect;
        FInput _fInput;
        string _sortingLayerName;
        string _PassWordForShow;
        string _PassWordUserInput = "";
        Int32 _CommandHistoryIndex = -1;
        List<string> _CommandHistory = new List<string>();
        private void Awake()
        {
            _FoldUnfoldButton.onClick.AddListener(FoldUnFold);
            _ClearButton.onClick.AddListener(Clear);
            _CloseButton.onClick.AddListener(Hide);
            _InputButton.onClick.AddListener(Input);
        }
        void Start()
        {
            Hide();
        }
        void Update()
        {
            if (CUnity.IsKeyDown(KeyCode.Return) || CUnity.IsKeyDown(KeyCode.KeypadEnter))
                Input();
            else if (CUnity.IsKeyDown(KeyCode.UpArrow))
                SetPrevCommand();
            else if (CUnity.IsKeyDown(KeyCode.DownArrow))
                SetNextCommand();
        }
        void OnDestroy()
        {
            _InputButton.onClick.RemoveAllListeners();
            _CloseButton.onClick.RemoveAllListeners();
            _ClearButton.onClick.RemoveAllListeners();
            _FoldUnfoldButton.onClick.RemoveAllListeners();
        }
        public void Init(FInput fInput, string sortingLayerName, string passWordForShow, string placeholderText)
        {
            _fInput = fInput;
            _sortingLayerName = sortingLayerName;
            _Canvas.sortingOrder = 10000;
            _PassWordForShow = passWordForShow;
            _Input.placeholder.GetComponent<Text>().text = placeholderText;
        }
        public void setCamera(Camera camera)
        {
            _Canvas.worldCamera = camera;
            _Canvas.planeDistance = _Canvas.worldCamera.nearClipPlane + 0.01f; // Near와 같으면 터치가 안될 때도 있음.
            _Canvas.sortingLayerName = _sortingLayerName;
        }
        public void InputPassword(char PasswordCharacter_)
        {
            _PassWordUserInput += PasswordCharacter_;
        }
        public bool ShowWithPassWordUserInput()
        {
            bool IsOk;
            if (_PassWordUserInput.Length >= _PassWordForShow.Length &&
                _PassWordUserInput.Substring(_PassWordUserInput.Length - _PassWordForShow.Length, _PassWordForShow.Length) == _PassWordForShow)
            {
                IsOk = true;
                Show();
            }
            else
            {
                IsOk = false;
            }

            ClearPassWord();

            return IsOk;
        }
        public void ClearPassWord()
        {
            _PassWordUserInput = "";
        }
        void _FocusInput()
        {
            _Input.Select();
            _Input.ActivateInputField();
        }
        public void Input()
        {
            if (_Input.text.Length > 0)
            {
                _fInput(_Input.text);
                _CommandHistory.Add(_Input.text);
                _CommandHistoryIndex = -1;
                _Input.text = "";
            }
            _FocusInput();
        }
        public void AddContent(string text)
        {
            _Content.text += text;
            StartCoroutine(ScrollToBottom());
        }
        IEnumerator ScrollToBottom()
        {
            yield return new WaitForEndOfFrame();
            _ContentScrollRect.verticalNormalizedPosition = 0f;
        }
        public void FoldUnFold()
        {
            if (_InputBar.activeSelf)
            {
                _InputBar.SetActive(false);
                _Body.SetActive(false);
                _FoldUnfoldButton.GetComponentInChildren<Text>().text = "Unfold";
            }
            else
            {
                _InputBar.SetActive(true);
                _Body.SetActive(true);
                _FoldUnfoldButton.GetComponentInChildren<Text>().text = "Fold";
            }
        }
        public void Clear()
        {
            _Content.text = "";
        }
        public void Show()
        {
            gameObject.SetActive(true);
            _FocusInput();
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        public void SetPrevCommand()
        {
            if (_CommandHistory.Count == 0 || _CommandHistoryIndex == 0)
                return;

            if (_CommandHistoryIndex == -1)
                _CommandHistoryIndex = _CommandHistory.Count - 1;
            else
                --_CommandHistoryIndex;

            _Input.text = _CommandHistory[_CommandHistoryIndex];
        }
        public void SetNextCommand()
        {
            if (_CommandHistory.Count == 0 || _CommandHistoryIndex == -1 || _CommandHistoryIndex == _CommandHistory.Count - 1)
                return;

            ++_CommandHistoryIndex;

            _Input.text = _CommandHistory[_CommandHistoryIndex];
        }
    }
}