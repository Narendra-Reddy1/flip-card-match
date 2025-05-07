using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DG.Tweening;
using UnityEngine;

namespace CardGame
{
    public class ScreenFader : MonoBehaviour
    {
        #region Variables

        [SerializeField] private bool _dontDestroyOnLoad = true;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private CanvasGroup _fader;
        [SerializeField] private Transform _aniamtedBg;
        [SerializeField] private Transform _gameTxt;
        private static ScreenFader instance;
        #endregion Variables

        #region Unity Events
        private static System.Action<bool> ToggleControlledLoadingScreen = default;
        private void Awake()
        {
            _initialPose = _aniamtedBg.transform.localPosition;
            ToggleControlledLoadingScreen += OnControlledLoadingScreenRequested;

        }
        private void OnDestroy()
        {
            ToggleControlledLoadingScreen -= OnControlledLoadingScreenRequested;
        }
        void OnEnable()
        {
            GlobalEventHandler.AddListener(EventID.RequestQuickLoadingScreen, Callback_On_Quick_Fade_Requested);
        }
        void OnDisable()
        {
            GlobalEventHandler.RemoveListener(EventID.RequestQuickLoadingScreen, Callback_On_Quick_Fade_Requested);
        }


        #endregion Unity Events

        #region Public Methods

        public static void ToggleControlledFadeAnim(bool value)
        {
            ToggleControlledLoadingScreen?.Invoke(value);
        }
        #endregion Public Methods

        #region Private Methods
        Vector3 _targetPose = new Vector3(219.619995f, 639.619995f, 0);
        Vector3 _initialPose;
        private void _AnimateItems()
        {
            _aniamtedBg.transform.DOLocalMove(_targetPose, 3).From(_initialPose).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            _gameTxt.transform.DOScale(1.3f, 2).From(1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
        private void _KillAniamtions()
        {
            DOTween.Kill(_aniamtedBg);
            DOTween.Kill(_gameTxt);
        }
        private bool _isControlledLoading = false;
        private void _ShowControlledLoading()
        {
            if (_isControlledLoading) return;
            _AnimateItems();
            _canvas.enabled = true;
            _isControlledLoading = true;
            _fader.DOFade(1f, .1f);

        }
        private void _DisableControlledLoading()
        {
            if (!_isControlledLoading) return;

            _fader.DOFade(0, .7f).SetDelay(Random.Range(0.2f, 0.4f)).onComplete += () =>
            {
                _isControlledLoading = false;
                _canvas.enabled = false;
                _KillAniamtions();
                _fader.alpha = 0;

                DOTween.Kill(_fader);
                GlobalEventHandler.TriggerEvent(EventID.OnLoadingComplete);
            };
        }




        private void OnControlledLoadingScreenRequested(bool value)
        {
            if (value)
                _ShowControlledLoading();
            else
                _DisableControlledLoading();
        }
        #endregion Private Methods

        #region Callbacks

        private void Callback_On_Quick_Fade_Requested(object obj)
        {
            _AnimateItems();
            _canvas.enabled = true;
            _fader.DOFade(1, 0.1f).From(0).onComplete += () =>
            {
                _fader.DOFade(0, .5f).From(1).SetDelay(1.5f).onComplete += () =>
                {
                    _canvas.enabled = false;
                    _KillAniamtions();
                };
            };
        }

        #endregion Callbacks

    }
}
