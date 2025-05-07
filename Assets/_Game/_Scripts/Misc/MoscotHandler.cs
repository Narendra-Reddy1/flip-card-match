using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CardGame
{
    public class MoscotHandler : MonoBehaviour
    {

        [TextArea]
        [SerializeField] private List<string> _dialogues;//could use a SO here.
        [SerializeField] private TextMeshProUGUI _dialogueTxt;
        [SerializeField] private Transform _dialogueBox;

        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.05f);
        private Vector3 _movePose;
        private Coroutine _typingCoroutine;
        void Start()
        {
            _movePose = new Vector3(_dialogueBox.localPosition.x, _dialogueBox.localPosition.y + 10, _dialogueBox.position.z);
        }
        void OnEnable()
        {
            GlobalEventHandler.AddListener(EventID.OnPlayLevelRequested, Callback_On_Level_Started);
        }
        void OnDisable()
        {
            GlobalEventHandler.RemoveListener(EventID.OnPlayLevelRequested, Callback_On_Level_Started);
        }
        public void Init()
        {
            var dialogue = string.Empty;
            if (GlobalVariables.isFirstSession)
                dialogue = _dialogues[0];
            else
            {
                dialogue = _dialogues[Random.Range(1, _dialogues.Count)];
            }
            _dialogueBox.DOScale(1, 0.35f).From(0.2f).SetEase(Ease.OutBounce).onComplete += () =>
            {
                _typingCoroutine = StartCoroutine(_ShowTypeWritingEffect(dialogue, () =>
                   {
                       _dialogueBox.transform.DOLocalMove(_movePose, 1.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
                   }));
            };
        }



        private IEnumerator _ShowTypeWritingEffect(string txt, System.Action onTextComplete = null)
        {

            _dialogueTxt.SetText(string.Empty); // Clear the current text
            string resultText = string.Empty; // Final visible text being built
            int startTagIndex;
            int startTagEndIndex;

            int endTagIndex = -1;
            int endTagEndIndex = -1;
            int i = 0;
            while (i < txt.Length)
            {
                if (txt[i] != '<') // Check if it's the start of an HTML tag
                {
                    resultText += txt[i];
                    _dialogueTxt.text = resultText;
                    i++;
                }
                else
                {
                    startTagIndex = i;
                    startTagEndIndex = txt.IndexOf('>', startTagIndex);
                    if (startTagEndIndex == -1) break; // Handle broken tags

                    string startTag = txt.Substring(startTagIndex, startTagEndIndex - startTagIndex + 1);
                    resultText += startTag;

                    i = startTagEndIndex + 1;

                    // Find the matching end tag
                    while (i < txt.Length && txt[i] != '<')
                    {
                        resultText += txt[i];
                        _dialogueTxt.text = resultText;
                        i++;
                        yield return _waitForSeconds;
                    }

                    if (i < txt.Length && txt[i] == '<')
                    {
                        endTagIndex = i;
                        endTagEndIndex = txt.IndexOf('>', endTagIndex);
                        if (endTagEndIndex == -1) break;

                        string endTag = txt.Substring(endTagIndex, endTagEndIndex - endTagIndex + 1);
                        resultText += endTag;
                        i = endTagEndIndex + 1;
                    }
                }
                yield return _waitForSeconds;
            }
            onTextComplete?.Invoke();
            _dialogueTxt.SetText(txt);
        }

        private void _ResetState()
        {
            _dialogueTxt.SetText(string.Empty);
            DOTween.Kill(_dialogueBox);
            if (_typingCoroutine != null)
                StopCoroutine(_typingCoroutine);
            _dialogueBox.localScale = Vector3.zero;
        }
        private void Callback_On_Level_Started(object args)
        {
            _ResetState();
        }
    }
}
