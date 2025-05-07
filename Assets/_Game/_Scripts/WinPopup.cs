using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class WinPopup : MonoBehaviour
    {
        #region Variables
        [Header("WIN")]
        [SerializeField] private TextMeshProUGUI _scoreTxt;
        [SerializeField] private TextMeshProUGUI _streakTxt;


        [Header("POPUP")]
        [SerializeField] private List<GameObject> _confettiBlasts;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private CanvasGroup _transparentBG;
        [SerializeField] private CanvasGroup _popup;
        [SerializeField] private CustomButton _nextBtn;

        private event System.Action _onNextClick;

        #endregion Variables

        #region Unity Events
        void OnEnable()
        {
            _nextBtn.interactable = false;
            _nextBtn.onClick.AddListener(_OnNextBtnClicked);
        }
        void OnDisable()
        {
            _nextBtn.onClick.RemoveListener(_OnNextBtnClicked);
        }
        #endregion Unity Events

        #region Public Methods
        public void Init(int score, int streak, System.Action onNextClick = null)
        {
            _scoreTxt.SetText(score.ToString());
            _streakTxt.SetText(streak.ToString() + "X");
            this._onNextClick = onNextClick;
        }
        public void ShowPopup(System.Action onPopupShown = null)
        {
            _canvas.enabled = true;
            gameObject.SetActive(true);
            StartCoroutine(_ShowPopupWithEffect(onPopupShown));
        }
        public void HidePopup()
        {
            _transparentBG.DOFade(0, .5f).From(1);
            _popup.DOFade(0, .5f).From(1f);
            _popup.transform.DOScale(0, 0.3f).From(1f).onComplete += () =>
            {
                _canvas.enabled = false;
                gameObject.SetActive(false);
            };
        }
        #endregion Public Methods

        #region Private Methods
        private IEnumerator _ShowPopupWithEffect(System.Action onComplete = null)
        {
            GlobalEventHandler.TriggerEvent(EventID.RequestToPlaySFXWithId, AudioID.LevelCompleteSFX);
            if (_confettiBlasts.Count > 0)
            {
                foreach (GameObject particle in _confettiBlasts)
                    particle.SetActive(false);

                foreach (GameObject particle in _confettiBlasts)
                {
                    yield return new WaitForSeconds(.15f);
                    particle.SetActive(true);
                }
                yield return new WaitForSeconds(0.5f);
            }
            _nextBtn.interactable = true;
            _transparentBG.DOFade(1, .3f).From(0);
            _popup.DOFade(1, .2f).From(0.2f);
            _popup.transform.DOScale(1, 0.3f).From(0.3f).SetEase(Ease.OutBack).onComplete += () => { onComplete?.Invoke(); };
        }
        private void _OnNextBtnClicked()
        {
            ///hide popup...
            /// load for new level...
            GlobalVariables.highestUnlockedLevelIndex++;
            PlayerDataManager.instance.ClearLevelData();
            _onNextClick?.Invoke();
            HidePopup();
        }

        #endregion Private Methods

        #region Callbacks


        #endregion Callbacks
    }
}
